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

        public async Task<Recipe?> GetRecipeByIdAsync(Guid id)
        {
            return await _dbContext.Recipes.FindAsync(id);
        }

        public async Task<IEnumerable<Recipe>> GetRecipesByUserIdAsync(Guid userId)
        {
            return await _dbContext.Recipes
                .Where(r => r.AuthorUserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Recipe>> GetAllRecipesAsync()
        {
            return await _dbContext.Recipes.ToListAsync();
        }

        public async Task<bool> CreateRecipeAsync(Recipe recipe)
        {
            if (recipe.Id == Guid.Empty)
            {
                recipe.Id = Guid.NewGuid();
            }

            await _dbContext.Recipes.AddAsync(recipe);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRecipeAsync(Recipe recipe)
        {
            _dbContext.Recipes.Update(recipe);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRecipeAsync(Guid recipeId)
        {
            var recipe = await _dbContext.Recipes.FindAsync(recipeId);
            if (recipe == null)
            {
                return false;
            }

            _dbContext.Recipes.Remove(recipe);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Recipe>> SearchRecipesAsync(string searchTerm)
        {
            return await _dbContext.Recipes
                .Where(r => r.Title.Contains(searchTerm) || r.Description.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Recipe>> GetRecipesByDietaryTagAsync(DietaryTag tag)
        {
            // This is trickier with EF Core and JSON conversion
            // We'll need to load all and filter in memory
            var allRecipes = await _dbContext.Recipes.ToListAsync();
            return allRecipes.Where(r => r.DietaryTags.Contains(tag));
        }
    }
}