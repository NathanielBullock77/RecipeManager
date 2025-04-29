using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using RecipeManager.Application.Services;
using RecipeManager.Core.Models;

namespace RecipeManager.MAUI.Views
{
    public partial class BrowseRecipesPage : ContentPage
    {
        private readonly IRecipeService _recipeService;

        public ObservableCollection<Recipe> Recipes { get; } = new();
        public string SearchTerm { get; set; }
        public ICommand SearchCommand { get; }

        public BrowseRecipesPage(IRecipeService recipeService)
        {
            InitializeComponent();
            _recipeService = recipeService;

            SearchCommand = new Command(async () =>
            {
                Recipes.Clear();
                var results = await _recipeService.BrowseRecipesAsync(SearchTerm, null);
                foreach (var r in results)
                    Recipes.Add(r);
            });

            BindingContext = this;
        }

        private async void OnViewRecipeClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Guid id)
            {
                await Shell.Current.GoToAsync(
                    nameof(RecipeDetailPage),
                    new Dictionary<string, object> { { "recipeId", id } }
                );
            }
        }
    }
}
