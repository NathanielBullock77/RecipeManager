using System;
using System.Collections.Generic;

namespace RecipeManager.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public List<DietaryTag> DietaryPreferences { get; set; } = new List<DietaryTag>();
        public List<Guid> FavoriteRecipeIds { get; set; } = new List<Guid>();
    }
} 