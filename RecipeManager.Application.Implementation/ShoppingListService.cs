using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecipeManager.Application.Services;
using RecipeManager.Core.Models;

namespace RecipeManager.Application.Implementation
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly IMealPlanService _mealPlanService;

        public ShoppingListService(IMealPlanService mealPlanService)
        {
            _mealPlanService = mealPlanService;
        }

        public Task<List<Ingredient>> GetCurrentShoppingListAsync(Guid userId)
        {
            // Generate shopping list for the current week
            var weekStartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            return _mealPlanService.GenerateShoppingListForWeekAsync(userId, weekStartDate);
        }

        public Task SaveShoppingListAsync(Guid userId, List<Ingredient> items)
        {
            throw new NotImplementedException();
        }
    }
} 