using Microsoft.EntityFrameworkCore;
using RecipeManager.Core.Models;
using RecipeManager.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManager.Infrastructure.Persistence.Implementation.SQLite
{
    public class SQLiteMealPlanRepository : IMealPlanRepository
    {
        private readonly RecipeManagerDbContext _dbContext;

        public SQLiteMealPlanRepository(RecipeManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<MealPlanEntry>> GetUserMealPlanForDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            return await _dbContext.MealPlans
                .Where(mp => mp.UserId == userId && mp.Date >= startDate && mp.Date <= endDate)
                .ToListAsync();
        }

        public async Task<MealPlanEntry?> GetUserMealPlanEntryAsync(Guid userId, DateTime date, Guid recipeId)
        {
            return await _dbContext.MealPlans
                .FirstOrDefaultAsync(mp => mp.UserId == userId && mp.Date.Date == date.Date && mp.RecipeId == recipeId);
        }

        public async Task AddAsync(MealPlanEntry entry)
        {
            if (entry.Id == Guid.Empty)
            {
                entry.Id = Guid.NewGuid();
            }

            await _dbContext.MealPlans.AddAsync(entry);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid entryId)
        {
            var mealPlan = await _dbContext.MealPlans.FindAsync(entryId);
            if (mealPlan != null)
            {
                _dbContext.MealPlans.Remove(mealPlan);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}