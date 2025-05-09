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
            // using test user for development
            var user = await _authService.LoginAsync("testuser", "your_test_password");
            if (user != null)
            {
                Preferences.Default.Set("user_id", user.Id.ToString());
                await DisplayAlert("Success", $"Logged in as {user.Username}", "OK");
                // In a real app, you would navigate to a user dashboard or recipe listing

                await Shell.Current.GoToAsync("//app/home");
            }
            else
            {
                await DisplayAlert("Error", "Login failed", "OK");
            }
        }


    }
} 