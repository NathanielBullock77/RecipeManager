using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using RecipeManager.Application.Services;
using RecipeManager.Core.Models;
using System.Collections.Generic;

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
                await ReloadRecipes();
            });

            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ReloadRecipes();
        }

        async Task ReloadRecipes()
        {
            Recipes.Clear();
            var tags = new List<DietaryTag>();
            if (KetoFilterCheck.IsChecked) tags.Add(DietaryTag.Keto);
            if (VegetarianFilterCheck.IsChecked) tags.Add(DietaryTag.Vegetarian);
            if (VeganFilterCheck.IsChecked) tags.Add(DietaryTag.Vegan);

            var results = await _recipeService.BrowseRecipesAsync(SearchTerm, tags);
            foreach (var r in results)
                Recipes.Add(r);
        }

        private async void OnFilterChanged(object sender, CheckedChangedEventArgs e)
        {
            await ReloadRecipes();
        }

        private async void OnViewRecipeClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Guid id)
            {
                await Shell.Current.GoToAsync($"{nameof(RecipeDetailPage)}?recipeId={id}");
            }
        }
    }
}
