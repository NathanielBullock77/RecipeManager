using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecipeManager.Core.Models;

namespace RecipeManager.Application.Services
{
    public interface IUserService
    {
        Task<User?> GetUserProfileAsync(Guid userId);
        Task<bool> UpdateDietaryPreferencesAsync(Guid userId, List<DietaryTag> preferences);
    }
} 