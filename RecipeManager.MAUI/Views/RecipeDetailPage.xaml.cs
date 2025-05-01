using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using RecipeManager.Application.Services;
using RecipeManager.Core.Models;

namespace RecipeManager.MAUI.Views
{
    [QueryProperty(nameof(RecipeId), "recipeId")]
    public partial class RecipeDetailPage : ContentPage
    {
        private readonly IRecipeService _recipeService;
        private readonly IMealPlanService _mealPlanService;

        public string FavoriteButtonText { get; set; }  //pfor favorite button text

        public Recipe Recipe { get; set; }


        public string RecipeId
        {
            set => LoadRecipe(value);
        }

        public RecipeDetailPage(
            IRecipeService recipeService,
            IMealPlanService mealPlanService)
        {
            InitializeComponent();
            _recipeService = recipeService;
            _mealPlanService = mealPlanService;
            BindingContext = this;
        }

        private async void LoadRecipe(string idString)
        {
            if (!Guid.TryParse(idString, out var id))
                return;

            Recipe = await _recipeService.GetRecipeDetailsAsync(id);
            OnPropertyChanged(nameof(Recipe));
            await UpdateFavoriteButtonTextAsync();
        }

        private async void OnAddToMealPlanClicked(object sender, EventArgs e)
        {
            var uidStr = Preferences.Default.Get<string>("user_id", string.Empty);
            if (string.IsNullOrWhiteSpace(uidStr) || !Guid.TryParse(uidStr, out var userId))
                return;

            if (Recipe == null)
                return;

            await _mealPlanService.AddRecipeToDayAsync(userId, DateTime.Today, Recipe.Id);
            await DisplayAlert("Success", "Added to meal plan.", "OK");
        }

        async Task UpdateFavoriteButtonTextAsync()
        {
            var uidStr = Preferences.Default.Get<string>("user_id", "");
            if (!Guid.TryParse(uidStr, out var userId) || Recipe == null)
                return;

            var favs = await _recipeService.GetFavoriteRecipesAsync(userId);
            bool isFav = favs.Any(r => r.Id == Recipe.Id);

            FavoriteButtonText = isFav ? "Remove Favorite" : "Add Favorites";
            OnPropertyChanged(nameof(FavoriteButtonText));
        }

        async void OnToggleFavoriteClicked(object sender, EventArgs e)
        {
            var uidStr = Preferences.Default.Get<string>("user_id", "");
            if (!Guid.TryParse(uidStr, out var userId) || Recipe == null)
                return;

            await _recipeService.ToggleFavoriteAsync(userId, Recipe.Id);
            await UpdateFavoriteButtonTextAsync();
        }
    



        async void OnDeleteRecipeClicked(object sender, EventArgs e)
        {
            var uidStr = Preferences.Default.Get("user_id", "");
            if (!Guid.TryParse(uidStr, out var userId) || Recipe == null) return;
            await _recipeService.DeleteRecipeAsync(Recipe.Id, userId);
            await Shell.Current.GoToAsync("..");
        }

        async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
