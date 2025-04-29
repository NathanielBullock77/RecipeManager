using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecipeManager.Application.Services;
using RecipeManager.Core.Models;
using RecipeManager.Infrastructure.Persistence.Repositories;

namespace RecipeManager.Application.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User?> GetUserProfileAsync(Guid userId)
        {
            return _userRepository.GetByIdAsync(userId);
        }

        public async Task<bool> UpdateDietaryPreferencesAsync(Guid userId, List<DietaryTag> preferences)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.DietaryPreferences = preferences;
            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
} 