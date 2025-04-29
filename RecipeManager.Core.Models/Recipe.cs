using System;
using System.Collections.Generic;

namespace RecipeManager.Core.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CookTimeMinutes { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<string> Instructions { get; set; } = new List<string>();
        public List<DietaryTag> DietaryTags { get; set; } = new List<DietaryTag>();
        public Guid? AuthorUserId { get; set; }
    }
} 