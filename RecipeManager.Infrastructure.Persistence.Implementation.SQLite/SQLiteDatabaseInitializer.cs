using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeManager.Core.Models;
using RecipeManager.Infrastructure.Persistence.Repositories;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RecipeManager.Infrastructure.Persistence.Implementation.SQLite
{
    public static class SQLiteDatabaseInitializer
    {
        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            // Get the database context
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<RecipeManagerDbContext>();
            
            // Create database and apply migrations
            await dbContext.Database.MigrateAsync();
            
            // Seed initial data if the database is empty
            await SeedInitialDataAsync(dbContext);
        }
        
        private static async Task SeedInitialDataAsync(RecipeManagerDbContext dbContext)
        {
            // Only seed data if the Users table is empty
            if (!await dbContext.Users.AnyAsync())
            {
                // Add a test user
                var testUser = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "test@example.com",
                    PasswordHash = "5f4dcc3b5aa765d61d8327deb882cf99", // "password"
                    FirstName = "Test",
                    LastName = "User",
                    DateRegistered = DateTime.Now
                };
                
                await dbContext.Users.AddAsync(testUser);
                
                // Add a few sample recipes
                var sampleRecipe1 = new Recipe
                {
                    Id = Guid.NewGuid(),
                    Title = "Simple Pasta",
                    Description = "A quick and easy pasta dish",
                    CookTimeMinutes = 20,
                    AuthorUserId = testUser.Id,
                    Ingredients = new System.Collections.Generic.List<Ingredient>
                    {
                        new Ingredient { Name = "Pasta", Quantity = "200g" },
                        new Ingredient { Name = "Olive Oil", Quantity = "2 tbsp" },
                        new Ingredient { Name = "Garlic", Quantity = "2 cloves" },
                        new Ingredient { Name = "Parmesan", Quantity = "50g" }
                    },
                    Instructions = new System.Collections.Generic.List<string>
                    {
                        "Boil pasta according to package instructions",
                        "Heat olive oil in a pan and add minced garlic",
                        "Drain pasta and add to the pan",
                        "Stir in grated parmesan and serve"
                    },
                    DietaryTags = new System.Collections.Generic.List<DietaryTag>
                    {
                        DietaryTag.Vegetarian
                    }
                };
                
                var sampleRecipe2 = new Recipe
                {
                    Id = Guid.NewGuid(),
                    Title = "Caesar Salad",
                    Description = "Classic Caesar salad with homemade dressing",
                    CookTimeMinutes = 15,
                    AuthorUserId = testUser.Id,
                    Ingredients = new System.Collections.Generic.List<Ingredient>
                    {
                        new Ingredient { Name = "Romaine Lettuce", Quantity = "1 head" },
                        new Ingredient { Name = "Croutons", Quantity = "1 cup" },
                        new Ingredient { Name = "Parmesan", Quantity = "50g" },
                        new Ingredient { Name = "Caesar Dressing", Quantity = "4 tbsp" }
                    },
                    Instructions = new System.Collections.Generic.List<string>
                    {
                        "Wash and chop the lettuce",
                        "Add croutons and grated parmesan",
                        "Drizzle with dressing and toss to combine"
                    },
                    DietaryTags = new System.Collections.Generic.List<DietaryTag>
                    {
                        DietaryTag.Vegetarian
                    }
                };
                
                await dbContext.Recipes.AddRangeAsync(sampleRecipe1, sampleRecipe2);
                
                // Add a sample meal plan
                var mealPlan = new MealPlanEntry
                {
                    Id = Guid.NewGuid(),
                    UserId = testUser.Id,
                    RecipeId = sampleRecipe1.Id,
                    Date = DateTime.Today,
                    MealType = "Dinner"
                };
                
                await dbContext.MealPlans.AddAsync(mealPlan);
                
                // Save all changes
                await dbContext.SaveChangesAsync();
            }
        }
        
        public static string GetDatabasePath(string filename = "recipemanager.db")
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appDataDir = Path.Combine(appDataPath, "RecipeManager");
            
            if (!Directory.Exists(appDataDir))
            {
                Directory.CreateDirectory(appDataDir);
            }
            
            return Path.Combine(appDataDir, filename);
        }
    }
}