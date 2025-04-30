using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using RecipeManager.Application.Implementation;
using RecipeManager.Application.Services;
using RecipeManager.Infrastructure.Persistence.Implementation.SQLite;
using RecipeManager.MAUI.Views;
using System.Threading.Tasks;

namespace RecipeManager.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register SQLite Infrastructure services
        builder.Services.AddSQLitePersistence();

        // Register Application services
        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddTransient<IRecipeService, RecipeService>();
        builder.Services.AddTransient<IMealPlanService, MealPlanService>();
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IShoppingListService, ShoppingListService>();

        // Register Pages
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<BrowseRecipesPage>();
        builder.Services.AddTransient<RecipeDetailPage>();
        builder.Services.AddTransient<AddRecipePage>();
        builder.Services.AddTransient<MealPlanPage>();
        builder.Services.AddTransient<ShoppingListPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<ForgotPasswordPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        
        // Initialize the SQLite database
        Task.Run(async () => await SQLiteDatabaseInitializer.InitializeDatabaseAsync(app.Services));
        
        return app;
    }
}