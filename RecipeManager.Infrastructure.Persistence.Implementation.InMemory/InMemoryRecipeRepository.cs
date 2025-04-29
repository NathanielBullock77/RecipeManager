using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeManager.Core.Models;
using RecipeManager.Infrastructure.Persistence.Repositories;

namespace RecipeManager.Infrastructure.Persistence.Implementation.InMemory
{
    public class InMemoryRecipeRepository : IRecipeRepository
    {
        private readonly ConcurrentDictionary<Guid, Recipe> _recipes = new ConcurrentDictionary<Guid, Recipe>();

        public Task<Recipe?> GetByIdAsync(Guid id)
        {
            _recipes.TryGetValue(id, out var recipe);
            return Task.FromResult(recipe);
        }

        public Task<IEnumerable<Recipe>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Recipe>>(_recipes.Values.ToList());
        }

        public Task<IEnumerable<Recipe>> FindAsync(string searchTerm, IEnumerable<DietaryTag>? dietaryTags)
        {
            var query = _recipes.Values.AsEnumerable();

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLowerInvariant();
                query = query.Where(r =>
                    r.Title.ToLowerInvariant().Contains(searchTerm) ||
                    r.Description.ToLowerInvariant().Contains(searchTerm) ||
                    r.Ingredients.Any(i => i.Name.ToLowerInvariant().Contains(searchTerm)));
            }

            // Apply dietary tags filter if provided
            if (dietaryTags != null && dietaryTags.Any())
            {
                var tagsList = dietaryTags.ToList();
                query = query.Where(r => tagsList.All(tag => r.DietaryTags.Contains(tag)));
            }

            return Task.FromResult(query);
        }

        public Task<IEnumerable<Recipe>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            var idsList = ids.ToList();
            var recipes = _recipes.Values.Where(r => idsList.Contains(r.Id)).ToList();
            return Task.FromResult<IEnumerable<Recipe>>(recipes);
        }

        public Task AddAsync(Recipe recipe)
        {
            if (recipe.Id == Guid.Empty)
            {
                recipe.Id = Guid.NewGuid();
            }

            _recipes.TryAdd(recipe.Id, recipe);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Recipe recipe)
        {
            _recipes[recipe.Id] = recipe;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            _recipes.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
} 