using System;

namespace RecipeManager.Core.Models
{
    public class MealPlanEntry
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public Guid RecipeId { get; set; }
    }
} 