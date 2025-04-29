using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using RecipeManager.Application.Services;
using RecipeManager.Core.Models;

namespace RecipeManager.MAUI.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly IRecipeService _recipeService;

        public ObservableCollection<Recipe> FavoriteRecipes { get; } = new();
        public ObservableCollection<Recipe> SuggestedRecipes { get; } = new();

        public HomePage(IRecipeService recipeService)
        {
            InitializeComponent();
            _recipeService = recipeService;
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

          
            var uidStr = Preferences.Default.Get<string>("user_id", string.Empty);
            if (string.IsNullOrWhiteSpace(uidStr) || !Guid.TryParse(uidStr, out var userId))
                return;

            FavoriteRecipes.Clear();
            foreach (var r in await _recipeService.GetFavoriteRecipesAsync(userId))
                FavoriteRecipes.Add(r);

            SuggestedRecipes.Clear();
            foreach (var r in await _recipeService.GetSuggestedRecipesAsync(userId))
                SuggestedRecipes.Add(r);
        }

        private async void OnViewRecipeClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Guid id)
            {
                // pass recipeId into the detail page
                await Shell.Current.GoToAsync(
                    nameof(RecipeDetailPage),
                    new Dictionary<string, object> { { "recipeId", id } }
                );
            }
        }
    }
}
