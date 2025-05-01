using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using RecipeManager.Application.Services;
using RecipeManager.Core.Models;
using System.Collections.Generic;

namespace RecipeManager.MAUI.Views
{
    public partial class AddRecipePage : ContentPage
    {
        private readonly IRecipeService _recipeService;

        public Recipe NewRecipe { get; set; }
        public ObservableCollection<Ingredient> Ingredients { get; }

        public AddRecipePage(IRecipeService recipeService)
        {
            InitializeComponent();
            _recipeService = recipeService;

            NewRecipe = new Recipe
            {
                Ingredients = new List<Ingredient>(),
                DietaryTags = new List<DietaryTag>()
            };
            Ingredients = new ObservableCollection<Ingredient>();

            BindingContext = this;
        }

        private void OnAddIngredientClicked(object sender, EventArgs e)
        {
            Ingredients.Add(new Ingredient { Name = string.Empty, Quantity = string.Empty });
        }

        private void OnRemoveIngredientClicked(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btn.BindingContext is Ingredient ing && Ingredients.Contains(ing))
                Ingredients.Remove(ing);
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var uidStr = Preferences.Default.Get<string>("user_id", string.Empty);
            if (!Guid.TryParse(uidStr, out var userId))
                return;

            NewRecipe.Title = NameEntry.Text;
            NewRecipe.Description = DescriptionEditor.Text;


            NewRecipe.Ingredients = Ingredients.ToList();

            var tags = new List<DietaryTag>();
            if (KetoCheck.IsChecked) tags.Add(DietaryTag.Keto);
            if (VegetarianCheck.IsChecked) tags.Add(DietaryTag.Vegetarian);
            if (VeganCheck.IsChecked) tags.Add(DietaryTag.Vegan);
            NewRecipe.DietaryTags = tags;

            await _recipeService.AddRecipeAsync(NewRecipe, userId);

            await Shell.Current.GoToAsync(
              "//app/shopping",
              new Dictionary<string, object> { ["ingredients"] = NewRecipe.Ingredients }
            );
            await Shell.Current.GoToAsync("//app/home");

            NewRecipe = new Recipe
            {
                Ingredients = new List<Ingredient>(),
                DietaryTags = new List<DietaryTag>()
            };
            OnPropertyChanged(nameof(NewRecipe));

            NameEntry.Text = string.Empty;
            DescriptionEditor.Text = string.Empty;
            CookTimeEntry.Text = string.Empty;
            KetoCheck.IsChecked = false;
            VegetarianCheck.IsChecked = false;
            VeganCheck.IsChecked = false;
            Ingredients.Clear();
        }
    }
}
