using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RecipeManager.Infrastructure.Persistence.Implementation.SQLite
{
    public class RecipeManagerDbContextFactory : IDesignTimeDbContextFactory<RecipeManagerDbContext>
    {
        public RecipeManagerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RecipeManagerDbContext>();
            
            // Use a temporary SQLite database for design-time migrations
            optionsBuilder.UseSqlite("Data Source=RecipeManager.db");
            
            return new RecipeManagerDbContext(optionsBuilder.Options);
        }
    }
}