using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipeManager.Core.Models;
using RecipeManager.Infrastructure.Persistence.Repositories;

namespace RecipeManager.Infrastructure.Persistence.Implementation.InMemory
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<Guid, User> _users = new ConcurrentDictionary<Guid, User>();
        private readonly ConcurrentDictionary<string, Guid> _usernameIndex = new ConcurrentDictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, Guid> _emailIndex = new ConcurrentDictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);

        public InMemoryUserRepository()
        {
            // Pre-seed with a test user
            var testUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@example.com",
                // TODO: Implement proper password hashing using BCrypt.Net
                PasswordHash = "$2a$11$K3.pJMsH3ydm69LC4qVZXO9FdzJqbytctA.di7vWEGeA8o9YdLGuW", // "your_test_password"
                DietaryPreferences = new List<DietaryTag> { DietaryTag.Vegetarian },
                FavoriteRecipeIds = new List<Guid>()
            };

            AddAsync(testUser).GetAwaiter().GetResult();
        }

        public Task<User?> GetByIdAsync(Guid id)
        {
            _users.TryGetValue(id, out var user);
            return Task.FromResult(user);
        }

        public Task<User?> GetByUsernameAsync(string username)
        {
            if (_usernameIndex.TryGetValue(username, out var id) && _users.TryGetValue(id, out var user))
            {
                return Task.FromResult<User?>(user);
            }
            return Task.FromResult<User?>(null);
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            if (_emailIndex.TryGetValue(email, out var id) && _users.TryGetValue(id, out var user))
            {
                return Task.FromResult<User?>(user);
            }
            return Task.FromResult<User?>(null);
        }

        public Task AddAsync(User user)
        {
            if (user.Id == Guid.Empty)
            {
                user.Id = Guid.NewGuid();
            }

            if (_users.TryAdd(user.Id, user))
            {
                _usernameIndex.TryAdd(user.Username, user.Id);
                _emailIndex.TryAdd(user.Email, user.Id);
            }

            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user)
        {
            if (_users.TryGetValue(user.Id, out var existingUser))
            {
                // Update username index if changed
                if (!string.Equals(existingUser.Username, user.Username, StringComparison.OrdinalIgnoreCase))
                {
                    _usernameIndex.TryRemove(existingUser.Username, out _);
                    _usernameIndex.TryAdd(user.Username, user.Id);
                }

                // Update email index if changed
                if (!string.Equals(existingUser.Email, user.Email, StringComparison.OrdinalIgnoreCase))
                {
                    _emailIndex.TryRemove(existingUser.Email, out _);
                    _emailIndex.TryAdd(user.Email, user.Id);
                }

                _users[user.Id] = user;
            }

            return Task.CompletedTask;
        }
    }
} 