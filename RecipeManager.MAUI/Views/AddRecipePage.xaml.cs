using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        public ObservableCollection<Ingredient> Ingredients { get; }

        public AddRecipePage(IRecipeService recipeService)
        {
            InitializeComponent();
            _recipeService = recipeService;

            // init the recipe and collection
            NewRecipe = new Recipe
            {
                Ingredients = new System.Collections.Generic.List<Ingredient>()
            };
            Ingredients = new ObservableCollection<Ingredient>();

            BindingContext = this;
        }

        // Add Ingredient is clicked
        private void OnAddIngredientClicked(object sender, EventArgs e)
        {
            // add a blank ingredient to the ObservableCollection
            Ingredients.Add(new Ingredient { Name = string.Empty, Quantity = string.Empty });
        }

        // Remove button of an ingredient row is clicked
        private void OnRemoveIngredientClicked(object sender, EventArgs e)
        {
            // Button.BindingContext is the Ingredient instance
            var btn = (Button)sender;
            if (btn.BindingContext is Ingredient ing && Ingredients.Contains(ing))
                Ingredients.Remove(ing);
        }

        // Save Recipe is clicked
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // get current user
            var uidStr = Preferences.Default.Get<string>("user_id", string.Empty);
            if (!Guid.TryParse(uidStr, out var userId))
                return;

            // copy into NewRcipe
            NewRecipe.Ingredients = Ingredients.ToList();
            await _recipeService.AddRecipeAsync(NewRecipe, userId);
            var ingList = NewRecipe.Ingredients;

            // navigate into the Shopping tab, passing the ingredients list
            await Shell.Current.GoToAsync(
              "//app/shopping",
              new Dictionary<string, object> { ["ingredients"] = ingList }
            );

            // optionaly navigate back
            await Shell.Current.GoToAsync("//app/home");


            // Clear the text entered after save button pressed
            NewRecipe = new Recipe { Ingredients = new List<Ingredient>() };
            OnPropertyChanged(nameof(NewRecipe));

            NameEntry.Text = string.Empty;
            DescriptionEditor.Text = string.Empty;
            CookTimeEntry.Text = string.Empty;

            Ingredients.Clear();
        }
    }
}
