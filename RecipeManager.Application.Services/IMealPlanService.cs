using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecipeManager.Core.Models;

namespace RecipeManager.Application.Services
{
    public interface IMealPlanService
    {
        Task<IEnumerable<MealPlanEntry>> GetWeekMealPlanAsync(Guid userId, DateTime weekStartDate);
        Task<bool> AddRecipeToDayAsync(Guid userId, DateTime date, Guid recipeId);
        Task<bool> RemoveRecipeFromDayAsync(Guid userId, DateTime date, Guid recipeId);
        Task<List<Ingredient>> GenerateShoppingListForWeekAsync(Guid userId, DateTime weekStartDate);
    }
} 