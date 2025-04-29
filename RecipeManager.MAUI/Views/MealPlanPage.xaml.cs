using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using RecipeManager.Application.Services;
using RecipeManager.Core.Models;

namespace RecipeManager.MAUI.Views
{
    public class DayPlan
    {
        public string DayName { get; set; }
        public ObservableCollection<Recipe> Recipes { get; } = new();
    }

    public partial class MealPlanPage : ContentPage
    {
        private readonly IMealPlanService _mealPlanService;
        private readonly IRecipeService _recipeService;

        public ObservableCollection<DayPlan> WeekDays { get; } = new();

        public MealPlanPage(
            IMealPlanService mealPlanService,
            IRecipeService recipeService)
        {
            InitializeComponent();
            _mealPlanService = mealPlanService;
            _recipeService = recipeService;
            BindingContext = this;

            // make one DayPlan per day
            foreach (DayOfWeek dow in Enum.GetValues<DayOfWeek>())
                WeekDays.Add(new DayPlan { DayName = dow.ToString() });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var uidStr = Preferences.Default.Get<string>("user_id", string.Empty);
            if (string.IsNullOrWhiteSpace(uidStr) || !Guid.TryParse(uidStr, out var userId))
                return;

            var weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var entries = await _mealPlanService.GetWeekMealPlanAsync(userId, weekStart);

            // clear & refill
            foreach (var dp in WeekDays)
                dp.Recipes.Clear();

            foreach (var e in entries)
            {
                var idx = (int)e.Date.DayOfWeek;
                var recipe = await _recipeService.GetRecipeDetailsAsync(e.RecipeId);
                if (recipe != null)
                    WeekDays[idx].Recipes.Add(recipe);
            }
        }
    }
}
