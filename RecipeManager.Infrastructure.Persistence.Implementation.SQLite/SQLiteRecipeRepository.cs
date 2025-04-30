using Microsoft.EntityFrameworkCore;
using RecipeManager.Core.Models;
using RecipeManager.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManager.Infrastructure.Persistence.Implementation.SQLite
{
    public class SQLiteRecipeRepository : IRecipeRepository
    {
        private readonly RecipeManagerDbContext _dbContext;

        public SQLiteRecipeRepository(RecipeManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Recipe?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Recipes.FindAsync(id);
        }

        public async Task<IEnumerable<Recipe>> GetAllAsync()
        {
            return await _dbContext.Recipes.ToListAsync();
        }

        public async Task<IEnumerable<Recipe>> FindAsync(string searchTerm, IEnumerable<DietaryTag>? dietaryTags)
        {
            var query = _dbContext.Recipes.AsQueryable();
            
            // Filter by search term
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(r => r.Title.Contains(searchTerm) || 
                                        r.Description.Contains(searchTerm));
            }
            
            // Get all recipes and filter by dietary tags in memory
            // (since we're storing tags as JSON, we can't easily filter them in the database query)
            var recipes = await query.ToListAsync();
            
            // Filter by dietary tags if specified
            if (dietaryTags != null && dietaryTags.Any())
            {
                var tagsList = dietaryTags.ToList();
                recipes = recipes.Where(r => r.DietaryTags.Any(tag => tagsList.Contains(tag))).ToList();
            }
            
            return recipes;
        }

        public async Task<IEnumerable<Recipe>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            var idsList = ids.ToList();
            return await _dbContext.Recipes
                .Where(r => idsList.Contains(r.Id))
                .ToListAsync();
        }

        public async Task AddAsync(Recipe recipe)
        {
            if (recipe.Id == Guid.Empty)
            {
                recipe.Id = Guid.NewGuid();
            }

            await _dbContext.Recipes.AddAsync(recipe);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Recipe recipe)
        {
            _dbContext.Recipes.Update(recipe);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var recipe = await _dbContext.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _dbContext.Recipes.Remove(recipe);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}