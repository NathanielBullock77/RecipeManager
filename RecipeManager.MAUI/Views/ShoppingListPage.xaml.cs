using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using RecipeManager.Application.Services;
using RecipeManager.Core.Models;

namespace RecipeManager.MAUI.Views
{
    public partial class ShoppingListPage : ContentPage
    {
        private readonly IShoppingListService _shoppingListService;
        public ObservableCollection<Ingredient> ShoppingList { get; } = new();

        public ShoppingListPage(IShoppingListService shoppingListService)
        {
            InitializeComponent();
            _shoppingListService = shoppingListService;
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var uidStr = Preferences.Default.Get<string>("user_id", string.Empty);
            if (string.IsNullOrWhiteSpace(uidStr) || !Guid.TryParse(uidStr, out var userId))
                return;

            ShoppingList.Clear();
            var items = await _shoppingListService.GetCurrentShoppingListAsync(userId);
            foreach (var ing in items)
                ShoppingList.Add(ing);
        }
    }
}
