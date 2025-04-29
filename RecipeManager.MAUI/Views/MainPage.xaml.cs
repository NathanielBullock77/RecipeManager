using Microsoft.Maui.Controls;
using RecipeManager.Application.Services;
using Microsoft.Maui.Storage;
using System;

namespace RecipeManager.MAUI.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly IAuthService _authService;
        
        public MainPage(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // In a real app, you would show a login page
            // For demo purposes, we'll just use our test user
            var user = await _authService.LoginAsync("testuser", "your_test_password");
            if (user != null)
            {
                Preferences.Default.Set("user_id", user.Id.ToString());// store the GUID as string


                await DisplayAlert("Success", $"Logged in as {user.Username}", "OK");
                // In a real app, you would navigate to a user dashboard or recipe listing
                
                await Shell.Current.GoToAsync("HomePage");
            }
            else
            {
                await DisplayAlert("Error", "Login failed", "OK");
            }
        }

        private void OnBrowseRecipesClicked(object sender, EventArgs e)
        {
            // In a real app, you would navigate to a recipe browse page
           //  DisplayAlert("Info", "Browse Recipes feature will be implemented soon!", "OK");
            async void OnBrowseRecipesClicked(object s, EventArgs e)
            {
                await Shell.Current.GoToAsync("BrowseRecipesPage");
            }
        }
    }
} 