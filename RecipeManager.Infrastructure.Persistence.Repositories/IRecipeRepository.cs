using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecipeManager.Core.Models;

namespace RecipeManager.Infrastructure.Persistence.Repositories
{
    public interface IRecipeRepository
    {
        Task<Recipe?> GetByIdAsync(Guid id);
        Task<IEnumerable<Recipe>> GetAllAsync();
        Task<IEnumerable<Recipe>> FindAsync(string searchTerm, IEnumerable<DietaryTag>? dietaryTags);
        Task<IEnumerable<Recipe>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task AddAsync(Recipe recipe);
        Task UpdateAsync(Recipe recipe);
        Task DeleteAsync(Guid id);
    }
} 