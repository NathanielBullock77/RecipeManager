using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecipeManager.Core.Models;

namespace RecipeManager.Application.Services
{
    public interface IRecipeService
    {
        Task<Recipe?> GetRecipeDetailsAsync(Guid recipeId);
        Task<IEnumerable<Recipe>> BrowseRecipesAsync(string? searchTerm = null, IEnumerable<DietaryTag>? dietaryTags = null);
        Task<IEnumerable<Recipe>> GetFavoriteRecipesAsync(Guid userId);
        Task<bool> ToggleFavoriteAsync(Guid userId, Guid recipeId);
        Task<Guid> AddRecipeAsync(Recipe newRecipe, Guid userId);
        Task<bool> UpdateRecipeAsync(Recipe updatedRecipe, Guid userId);
        Task<bool> DeleteRecipeAsync(Guid recipeId, Guid userId);
        Task<IEnumerable<Recipe>> GetSuggestedRecipesAsync(Guid userId);
        Task<IEnumerable<Recipe>> GetRecipesByIdsAsync(IEnumerable<Guid> recipeIds);
    }
} 