using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeManager.Core.Models;
using RecipeManager.Infrastructure.Persistence.Repositories;

namespace RecipeManager.Infrastructure.Persistence.Implementation.InMemory
{
    public class InMemoryMealPlanRepository : IMealPlanRepository
    {
        private readonly ConcurrentDictionary<Guid, MealPlanEntry> _mealPlanEntries = new ConcurrentDictionary<Guid, MealPlanEntry>();

        public Task<IEnumerable<MealPlanEntry>> GetUserMealPlanForDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            var normalizedStartDate = startDate.Date;
            var normalizedEndDate = endDate.Date.AddDays(1).AddTicks(-1); // End of the day

            var entries = _mealPlanEntries.Values
                .Where(e => e.UserId == userId && e.Date >= normalizedStartDate && e.Date <= normalizedEndDate)
                .ToList();

            return Task.FromResult<IEnumerable<MealPlanEntry>>(entries);
        }

        public Task<MealPlanEntry?> GetUserMealPlanEntryAsync(Guid userId, DateTime date, Guid recipeId)
        {
            var normalizedDate = date.Date;

            var entry = _mealPlanEntries.Values
                .FirstOrDefault(e => e.UserId == userId && e.Date.Date == normalizedDate && e.RecipeId == recipeId);

            return Task.FromResult(entry);
        }

        public Task AddAsync(MealPlanEntry entry)
        {
            if (entry.Id == Guid.Empty)
            {
                entry.Id = Guid.NewGuid();
            }

            _mealPlanEntries.TryAdd(entry.Id, entry);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid entryId)
        {
            _mealPlanEntries.TryRemove(entryId, out _);
            return Task.CompletedTask;
        }
    }
} 