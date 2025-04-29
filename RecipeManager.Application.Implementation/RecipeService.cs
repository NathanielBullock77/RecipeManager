using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeManager.Application.Services;
using RecipeManager.Core.Models;
using RecipeManager.Infrastructure.Persistence.Repositories;

namespace RecipeManager.Application.Implementation
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUserRepository _userRepository;

        public RecipeService(IRecipeRepository recipeRepository, IUserRepository userRepository)
        {
            _recipeRepository = recipeRepository;
            _userRepository = userRepository;
        }

        public Task<Recipe?> GetRecipeDetailsAsync(Guid recipeId)
        {
            return _recipeRepository.GetByIdAsync(recipeId);
        }

        public Task<IEnumerable<Recipe>> BrowseRecipesAsync(string? searchTerm = null, IEnumerable<DietaryTag>? dietaryTags = null)
        {
            return _recipeRepository.FindAsync(searchTerm ?? string.Empty, dietaryTags);
        }

        public async Task<IEnumerable<Recipe>> GetFavoriteRecipesAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || !user.FavoriteRecipeIds.Any())
            {
                return Enumerable.Empty<Recipe>();
            }

            return await _recipeRepository.GetByIdsAsync(user.FavoriteRecipeIds);
        }

        public async Task<bool> ToggleFavoriteAsync(Guid userId, Guid recipeId)
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

            if (user.FavoriteRecipeIds.Contains(recipeId))
            {
                user.FavoriteRecipeIds.Remove(recipeId);
            }
            else
            {
                user.FavoriteRecipeIds.Add(recipeId);
            }

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<Guid> AddRecipeAsync(Recipe newRecipe, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            newRecipe.Id = Guid.NewGuid();
            newRecipe.AuthorUserId = userId;

            await _recipeRepository.AddAsync(newRecipe);
            return newRecipe.Id;
        }

        public async Task<bool> UpdateRecipeAsync(Recipe updatedRecipe, Guid userId)
        {
            var existingRecipe = await _recipeRepository.GetByIdAsync(updatedRecipe.Id);
            if (existingRecipe == null)
            {
                return false;
            }

            // Ensure the user is the author of the recipe
            if (existingRecipe.AuthorUserId != userId)
            {
                return false;
            }

            await _recipeRepository.UpdateAsync(updatedRecipe);
            return true;
        }

        public async Task<bool> DeleteRecipeAsync(Guid recipeId, Guid userId)
        {
            var existingRecipe = await _recipeRepository.GetByIdAsync(recipeId);
            if (existingRecipe == null)
            {
                return false;
            }

            // Ensure the user is the author of the recipe
            if (existingRecipe.AuthorUserId != userId)
            {
                return false;
            }

            await _recipeRepository.DeleteAsync(recipeId);
            return true;
        }

        public async Task<IEnumerable<Recipe>> GetSuggestedRecipesAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return Enumerable.Empty<Recipe>();
            }

            // A simple implementation that suggests recipes based on dietary preferences
            var allRecipes = await _recipeRepository.GetAllAsync();
            
            var suggestedRecipes = allRecipes
                .Where(r => !user.FavoriteRecipeIds.Contains(r.Id)) // Exclude already favorited recipes
                .Where(r => user.DietaryPreferences.All(tag => r.DietaryTags.Contains(tag))) // Match dietary preferences
                .Take(10) // Limit to 10 suggestions
                .ToList();

            return suggestedRecipes;
        }

        public Task<IEnumerable<Recipe>> GetRecipesByIdsAsync(IEnumerable<Guid> recipeIds)
        {
            return _recipeRepository.GetByIdsAsync(recipeIds);
        }
    }
} 