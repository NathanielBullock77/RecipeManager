using System;
using System.Threading.Tasks;
using RecipeManager.Core.Models;

namespace RecipeManager.Application.Services
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string email, string password);
        Task<bool> RequestPasswordResetAsync(string username, string email);
        Task<bool> ResetPasswordAsync(string identifier, string resetToken, string newPassword);
        Task<bool> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
    }
} 