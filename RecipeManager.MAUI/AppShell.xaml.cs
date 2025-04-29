using Microsoft.Maui.Controls;
using RecipeManager.MAUI.Views;

namespace RecipeManager.MAUI
{
    public partial class AppShell : Microsoft.Maui.Controls.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RecipeDetailPage), typeof(RecipeDetailPage));
            Routing.RegisterRoute(nameof(AddRecipePage), typeof(AddRecipePage));
            Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        }
    }
} 