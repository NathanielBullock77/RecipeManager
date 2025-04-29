using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using RecipeManager.Application.Implementation;
using RecipeManager.Application.Services;
using RecipeManager.Infrastructure.Persistence.Implementation.InMemory;
using RecipeManager.Infrastructure.Persistence.Repositories;
using RecipeManager.MAUI.Views;

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

        // Register Infrastructure services
        builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        builder.Services.AddSingleton<IRecipeRepository, InMemoryRecipeRepository>();
        builder.Services.AddSingleton<IMealPlanRepository, InMemoryMealPlanRepository>();

        // Register Application services
        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddTransient<IRecipeService, RecipeService>();
        builder.Services.AddTransient<IMealPlanService, MealPlanService>();
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IShoppingListService, ShoppingListService>();

        // Register Pages
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
} 