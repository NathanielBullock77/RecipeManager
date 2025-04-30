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

        public async Task<MealPlanEntry?> GetMealPlanByIdAsync(Guid id)
        {
            return await _dbContext.MealPlans.FindAsync(id);
        }

        public async Task<IEnumerable<MealPlanEntry>> GetMealPlansByUserIdAsync(Guid userId)
        {
            return await _dbContext.MealPlans
                .Where(mp => mp.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MealPlanEntry>> GetMealPlansByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            return await _dbContext.MealPlans
                .Where(mp => mp.UserId == userId && mp.Date >= startDate && mp.Date <= endDate)
                .ToListAsync();
        }

        public async Task<bool> CreateMealPlanAsync(MealPlanEntry mealPlan)
        {
            if (mealPlan.Id == Guid.Empty)
            {
                mealPlan.Id = Guid.NewGuid();
            }

            await _dbContext.MealPlans.AddAsync(mealPlan);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateMealPlanAsync(MealPlanEntry mealPlan)
        {
            _dbContext.MealPlans.Update(mealPlan);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMealPlanAsync(Guid mealPlanId)
        {
            var mealPlan = await _dbContext.MealPlans.FindAsync(mealPlanId);
            if (mealPlan == null)
            {
                return false;
            }

            _dbContext.MealPlans.Remove(mealPlan);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<MealPlanEntry>> GetMealPlansByRecipeIdAsync(Guid recipeId)
        {
            return await _dbContext.MealPlans
                .Where(mp => mp.RecipeId == recipeId)
                .ToListAsync();
        }
    }
}