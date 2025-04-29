using System;
using System.Threading.Tasks;
using RecipeManager.Core.Models;

namespace RecipeManager.Infrastructure.Persistence.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
} 