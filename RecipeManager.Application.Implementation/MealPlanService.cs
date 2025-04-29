using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeManager.Application.Services;
using RecipeManager.Core.Models;
using RecipeManager.Infrastructure.Persistence.Repositories;

namespace RecipeManager.Application.Implementation
{
    public class MealPlanService : IMealPlanService
    {
        private readonly IMealPlanRepository _mealPlanRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUserRepository _userRepository;

        public MealPlanService(
            IMealPlanRepository mealPlanRepository,
            IRecipeRepository recipeRepository,
            IUserRepository userRepository)
        {
            _mealPlanRepository = mealPlanRepository;
            _recipeRepository = recipeRepository;
            _userRepository = userRepository;
        }

        public Task<IEnumerable<MealPlanEntry>> GetWeekMealPlanAsync(Guid userId, DateTime weekStartDate)
        {
            var normalizedStartDate = weekStartDate.Date;
            var endDate = normalizedStartDate.AddDays(7);
            return _mealPlanRepository.GetUserMealPlanForDateRangeAsync(userId, normalizedStartDate, endDate);
        }

        public async Task<bool> AddRecipeToDayAsync(Guid userId, DateTime date, Guid recipeId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var recipe = await _recipeRepository.GetByIdAsync(recipeId);
            if (recipe == null)
            {
                return false;
            }

            // Check if this recipe is already planned for this day
            var existingEntry = await _mealPlanRepository.GetUserMealPlanEntryAsync(userId, date, recipeId);
            if (existingEntry != null)
            {
                return true; // Already added
            }

            var entry = new MealPlanEntry
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Date = date.Date,
                RecipeId = recipeId
            };

            await _mealPlanRepository.AddAsync(entry);
            return true;
        }

        public async Task<bool> RemoveRecipeFromDayAsync(Guid userId, DateTime date, Guid recipeId)
        {
            var entry = await _mealPlanRepository.GetUserMealPlanEntryAsync(userId, date, recipeId);
            if (entry == null)
            {
                return false;
            }

            await _mealPlanRepository.DeleteAsync(entry.Id);
            return true;
        }

        public async Task<List<Ingredient>> GenerateShoppingListForWeekAsync(Guid userId, DateTime weekStartDate)
        {
            var mealPlanEntries = await GetWeekMealPlanAsync(userId, weekStartDate);
            if (!mealPlanEntries.Any())
            {
                return new List<Ingredient>();
            }

            var recipeIds = mealPlanEntries.Select(e => e.RecipeId).Distinct().ToList();
            var recipes = await _recipeRepository.GetByIdsAsync(recipeIds);

            // Group similar ingredients and aggregate quantities
            var ingredientDictionary = new Dictionary<string, Ingredient>(StringComparer.OrdinalIgnoreCase);

            foreach (var recipe in recipes)
            {
                foreach (var ingredient in recipe.Ingredients)
                {
                    if (ingredientDictionary.TryGetValue(ingredient.Name, out var existingIngredient))
                    {
                        // For simplicity, we just append the quantities with a comma
                        // In a real app, you'd want to parse and normalize units
                        existingIngredient.Quantity += $", {ingredient.Quantity}";
                    }
                    else
                    {
                        ingredientDictionary[ingredient.Name] = new Ingredient
                        {
                            Name = ingredient.Name,
                            Quantity = ingredient.Quantity
                        };
                    }
                }
            }

            return ingredientDictionary.Values.ToList();
        }
    }
} 