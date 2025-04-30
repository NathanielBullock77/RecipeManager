using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeManager.Infrastructure.Persistence.Repositories;
using System;

namespace RecipeManager.Infrastructure.Persistence.Implementation.SQLite
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSQLitePersistence(this IServiceCollection services)
        {
            string dbPath = SQLiteDatabaseInitializer.GetDatabasePath();
            
            // Register DbContext
            services.AddDbContext<RecipeManagerDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));
            
            // Register repositories
            services.AddScoped<IUserRepository, SQLiteUserRepository>();
            services.AddScoped<IRecipeRepository, SQLiteRecipeRepository>();
            services.AddScoped<IMealPlanRepository, SQLiteMealPlanRepository>();
            
            return services;
        }
    }
}