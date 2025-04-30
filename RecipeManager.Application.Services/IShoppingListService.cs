using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecipeManager.Core.Models;

namespace RecipeManager.Application.Services
{
    public interface IShoppingListService
    {
        Task<List<Ingredient>> GetCurrentShoppingListAsync(Guid userId);

        // TODO: implement this to persist the updated shopping list to the DB
        Task SaveShoppingListAsync(Guid userId, List<Ingredient> items);
    }
} 