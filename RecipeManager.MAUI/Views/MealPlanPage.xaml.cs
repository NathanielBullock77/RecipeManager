using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
        readonly IMealPlanService _mealPlanService;
        readonly IRecipeService _recipeService;
        public ObservableCollection<DayPlan> WeekDays { get; } = new();

        public MealPlanPage(IMealPlanService mealPlanService, IRecipeService recipeService)
        {
            InitializeComponent();
            _mealPlanService = mealPlanService;
            _recipeService = recipeService;
            BindingContext = this;
            foreach (DayOfWeek dow in Enum.GetValues<DayOfWeek>())
                WeekDays.Add(new DayPlan { DayName = dow.ToString() });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadWeekPlan();
        }

        async Task LoadWeekPlan()
        {
            var uidStr = Preferences.Default.Get("user_id", "");
            if (!Guid.TryParse(uidStr, out var userId)) return;
            var weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var entries = await _mealPlanService.GetWeekMealPlanAsync(userId, weekStart);
            foreach (var dp in WeekDays) dp.Recipes.Clear();
            foreach (var e in entries)
            {
                var idx = (int)e.Date.DayOfWeek;
                var recipe = await _recipeService.GetRecipeDetailsAsync(e.RecipeId);
                if (recipe != null)
                    WeekDays[idx].Recipes.Add(recipe);
            }
        }

        async void OnMoveLeftClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Recipe r)
            {
                var from = WeekDays.ToList().FindIndex(dp => dp.Recipes.Contains(r));
                var to = Math.Max(0, from - 1);
                await MoveRecipe(r.Id, from, to);
            }
        }

        async void OnMoveRightClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Recipe r)
            {
                var from = WeekDays.ToList().FindIndex(dp => dp.Recipes.Contains(r));
                var to = Math.Min(6, from + 1);
                await MoveRecipe(r.Id, from, to);
            }
        }

        async Task MoveRecipe(Guid recipeId, int fromIndex, int toIndex)
        {
            var uidStr = Preferences.Default.Get("user_id", "");
            if (!Guid.TryParse(uidStr, out var userId)) return;
            var weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var fromDate = weekStart.AddDays(fromIndex);
            var toDate = weekStart.AddDays(toIndex);
            await _mealPlanService.RemoveRecipeFromDayAsync(userId, fromDate, recipeId);
            await _mealPlanService.AddRecipeToDayAsync(userId, toDate, recipeId);
            await LoadWeekPlan();
        }

        async void OnRemoveClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Recipe r)
            {
                var from = WeekDays.ToList().FindIndex(dp => dp.Recipes.Contains(r));
                var uidStr = Preferences.Default.Get("user_id", "");
                if (!Guid.TryParse(uidStr, out var userId)) return;
                var weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                var date = weekStart.AddDays(from);
                await _mealPlanService.RemoveRecipeFromDayAsync(userId, date, r.Id);
                await LoadWeekPlan();
            }
        }

    }
}
