using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using RecipeManager.Application.Services;
using RecipeManager.Core.Models;

namespace RecipeManager.MAUI.Views
{
    public partial class AddRecipePage : ContentPage
    {
        private readonly IRecipeService _recipeService;

        public Recipe NewRecipe { get; set; }

        public AddRecipePage(IRecipeService recipeService)
        {
            InitializeComponent();
            _recipeService = recipeService;

            NewRecipe = new Recipe
            {
                Ingredients = new List<Ingredient>()
            };
            BindingContext = this;
        }

        private void OnAddIngredientClicked(object sender, EventArgs e)
        {
            NewRecipe.Ingredients.Add(new Ingredient());
            OnPropertyChanged(nameof(NewRecipe));
        }

        private void OnRemoveIngredientClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Ingredient ing)
            {
                NewRecipe.Ingredients.Remove(ing);
                OnPropertyChanged(nameof(NewRecipe));
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var uidStr = Preferences.Default.Get<string>("user_id", string.Empty);
            if (string.IsNullOrWhiteSpace(uidStr) || !Guid.TryParse(uidStr, out var userId))
                return;

            await _recipeService.AddRecipeAsync(NewRecipe, userId);
            await Shell.Current.GoToAsync("..");
        }
    }
}
