using System;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using RecipeManager.Application.Services;

namespace RecipeManager.MAUI.Views
{
    public partial class ForgotPasswordPage : ContentPage
    {
        private readonly IAuthService _authService;
        public string Identifier { get; set; }

        public ForgotPasswordPage(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
            BindingContext = this;
        }

        public ICommand RequestResetCommand => new Command(async () =>
        {
            var ok = await _authService.RequestPasswordResetAsync(Identifier, Identifier);
            await DisplayAlert("OOps!", "It seems that thanos has snapped the password out of existance! Unfortunatly, nothing is developed to fix this! Blame the devs.", "I dont feel so good..."); ;
        });
    }
}
