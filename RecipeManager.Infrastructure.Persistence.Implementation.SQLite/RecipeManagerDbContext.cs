using Microsoft.EntityFrameworkCore;
using RecipeManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace RecipeManager.Infrastructure.Persistence.Implementation.SQLite
{
    public class RecipeManagerDbContext : DbContext
    {
        public RecipeManagerDbContext(DbContextOptions<RecipeManagerDbContext> options) 
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<MealPlanEntry> MealPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
            });

            // Configure Recipe entity with JSON conversion for collections
            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                
                // Store Ingredients as JSON
                entity.Property(e => e.Ingredients)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                        v => JsonSerializer.Deserialize<List<Ingredient>>(v, new JsonSerializerOptions()) ?? new List<Ingredient>());
                
                // Store Instructions as JSON
                entity.Property(e => e.Instructions)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                        v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()) ?? new List<string>());
                
                // Store DietaryTags as JSON
                entity.Property(e => e.DietaryTags)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                        v => JsonSerializer.Deserialize<List<DietaryTag>>(v, new JsonSerializerOptions()) ?? new List<DietaryTag>());
            });

            // Configure MealPlanEntry entity
            modelBuilder.Entity<MealPlanEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Date).IsRequired();
            });
        }
    }
}