using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecipeManager.Core.Models;

namespace RecipeManager.Infrastructure.Persistence.Repositories
{
    public interface IMealPlanRepository
    {
        Task<IEnumerable<MealPlanEntry>> GetUserMealPlanForDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<MealPlanEntry?> GetUserMealPlanEntryAsync(Guid userId, DateTime date, Guid recipeId);
        Task AddAsync(MealPlanEntry entry);
        Task DeleteAsync(Guid entryId);
    }
} 