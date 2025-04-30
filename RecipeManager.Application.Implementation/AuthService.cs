using System;
using System.Threading.Tasks;
using RecipeManager.Application.Services;
using RecipeManager.Core.Models;
using RecipeManager.Infrastructure.Persistence.Repositories;
using System.Diagnostics;
using System.Collections.Generic;

namespace RecipeManager.Application.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            // For development purposes only: hardcoded test user login
            if (username == "testuser" && password == "your_test_password")
            {
                // Try to find an existing test user first
                var existingUser = await _userRepository.GetByUsernameAsync(username);
                
                // If not found, create a test user on the fly
                if (existingUser == null)
                {
                    var testUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "testuser",
                        Email = "testuser@example.com",
                        PasswordHash = "$2a$11$K3.pJMsH3ydm69LC4qVZXO9FdzJqbytctA.di7vWEGeA8o9YdLGuW", // "your_test_password"
                        DietaryPreferences = new List<DietaryTag> { DietaryTag.Vegetarian },
                        FavoriteRecipeIds = new List<Guid>()
                    };
                    
                    await _userRepository.AddAsync(testUser);
                    return testUser;
                }
                
                return existingUser;
            }
            
            // Regular login process for non-test users
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                return null;
            }

            // TODO: Implement proper password verification using BCrypt.Net
            // bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            
            // Mock implementation for development
            bool isPasswordValid = password == "your_test_password" && 
                user.PasswordHash == "$2a$11$K3.pJMsH3ydm69LC4qVZXO9FdzJqbytctA.di7vWEGeA8o9YdLGuW";

            return isPasswordValid ? user : null;
        }

        public async Task<bool> RegisterAsync(string username, string email, string password)
        {
            // Check if username already exists
            var existingUserByUsername = await _userRepository.GetByUsernameAsync(username);
            if (existingUserByUsername != null)
            {
                return false;
            }

            // Check if email already exists
            var existingUserByEmail = await _userRepository.GetByEmailAsync(email);
            if (existingUserByEmail != null)
            {
                return false;
            }

            // TODO: Implement proper password hashing using BCrypt.Net
            // string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            string passwordHash = "$2a$11$K3.pJMsH3ydm69LC4qVZXO9FdzJqbytctA.di7vWEGeA8o9YdLGuW"; // Mock hash

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                DietaryPreferences = new List<DietaryTag>(),
                FavoriteRecipeIds = new List<Guid>()
            };

            await _userRepository.AddAsync(user);
            return true;
        }

        public Task<bool> RequestPasswordResetAsync(string username, string email)
        {
            // In a real implementation, this would:
            // 1. Verify the user exists
            // 2. Generate a password reset token
            // 3. Store the token with an expiration time
            // 4. Send an email with a reset link
            
            // Mock implementation
            return Task.FromResult(true);
        }

        public Task<bool> ResetPasswordAsync(string identifier, string resetToken, string newPassword)
        {
            // In a real implementation, this would:
            // 1. Verify the token exists and is valid
            // 2. Update the user's password
            // 3. Delete the used token
            
            // Mock implementation
            return Task.FromResult(true);
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            // TODO: Implement proper password verification using BCrypt.Net
            // bool isOldPasswordValid = BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash);
            
            // Mock implementation
            bool isOldPasswordValid = oldPassword == "your_test_password" && 
                user.PasswordHash == "$2a$11$K3.pJMsH3ydm69LC4qVZXO9FdzJqbytctA.di7vWEGeA8o9YdLGuW";
            
            if (!isOldPasswordValid)
            {
                return false;
            }

            // TODO: Implement proper password hashing using BCrypt.Net
            // user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordHash = "$2a$11$K3.pJMsH3ydm69LC4qVZXO9FdzJqbytctA.di7vWEGeA8o9YdLGuW"; // Mock hash for new password

            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}