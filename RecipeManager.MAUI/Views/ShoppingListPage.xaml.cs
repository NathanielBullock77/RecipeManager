using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using RecipeManager.Application.Services;
using RecipeManager.Core.Models;

namespace RecipeManager.MAUI.Views
{
    public partial class ShoppingListPage : ContentPage, IQueryAttributable
    {
        private readonly IShoppingListService _shoppingListService;
        private bool _haveQueryData;

        // 1) Init here so it's never null - causes crashes if null, for me at least
        public ObservableCollection<Ingredient> ShoppingItems { get; }
            = new ObservableCollection<Ingredient>();

        public ShoppingListPage(IShoppingListService shoppingListService)
        {
            InitializeComponent();
            _shoppingListService = shoppingListService;

            // 2) Bind XAML to this instance
            BindingContext = this;
        }

        // 3) If AddRecipePage gives a List<Ingredient>, use it
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (_haveQueryData) return;  

            if (query.TryGetValue("ingredients", out var obj)
                && obj is IEnumerable<Ingredient> list)
            {
                _haveQueryData = true;
                ShoppingItems.Clear();
                foreach (var ing in list)
                    ShoppingItems.Add(ing);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // 4) If no data came via the query, grab list for the current user, clear the list, and populat it with what was retrieved
            if (!_haveQueryData)
            {
                var uidStr = Preferences.Default.Get<string>("user_id", string.Empty);
                if (Guid.TryParse(uidStr, out var userId))
                {
                    var items = await _shoppingListService.GetCurrentShoppingListAsync(userId);
                    ShoppingItems.Clear();
                    foreach (var ing in items)
                        ShoppingItems.Add(ing);
                }
            }
        }

        private void OnAddItemClicked(object sender, EventArgs e)// adds a new shopping list item!
        {
            ShoppingItems.Add(new Ingredient { Name = string.Empty, Quantity = string.Empty });
        }

        private void OnRemoveItemClicked(object sender, EventArgs e) //removes a shopping list item!!
        {
            if (sender is Button btn && btn.BindingContext is Ingredient ing)
                ShoppingItems.Remove(ing);
        }

        private async void OnSaveListClicked(object sender, EventArgs e)
        {
            // retrieve current user
            var uidStr = Preferences.Default.Get<string>("user_id", string.Empty);
            if (!Guid.TryParse(uidStr, out var userId))
                return;

            // TODO: persist the list with db
            await _shoppingListService.SaveShoppingListAsync(
                userId,
                ShoppingItems.ToList()
            );

            await DisplayAlert("Saved", "Shopping list persisted.", "OK");
        }

    }
}
