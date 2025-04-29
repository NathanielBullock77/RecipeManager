using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using RecipeManager.Application.Services;
using RecipeManager.Core.Models;
using Microsoft.Maui.Storage;
using System.Threading.Tasks;

namespace RecipeManager.MAUI.Views
{
    public partial class SettingsPage : ContentPage
    {
        private readonly IUserService _userService;

        public bool IsDarkTheme { get; set; }
        public ObservableCollection<DietaryTag> AllDietaryTags { get; } = new();
        public Dictionary<DietaryTag, bool> DietaryMap { get; } = new();

        public SettingsPage(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
            BindingContext = this;

            // populate possible tags
            foreach (var tag in Enum.GetValues<DietaryTag>())
            {
                AllDietaryTags.Add(tag);
                DietaryMap[tag] = false;
            }

            LoadUserSettings();
        }

        private async void LoadUserSettings()
        {
            await DisplayAlert("Feature Not Implemented", "This feature is not yet implemented.", "OK");
            /* if (!App.Current.Properties.TryGetValue("UserId", out var uidObj) || uidObj is not Guid userId)
             {
                 return;
             }

             var user = await _userService.GetUserProfileAsync(userId);
             IsDarkTheme = Application.Current.UserAppTheme == OSAppTheme.Dark;
             OnPropertyChanged(nameof(IsDarkTheme));

             foreach (var pref in user?.DietaryPreferences ?? Array.Empty<DietaryTag>())
                 DietaryMap[pref] = true;

             OnPropertyChanged(nameof(DietaryMap)); */
        }

        private async Task OnThemeToggled(object sender, ToggledEventArgs e)
        {
            await DisplayAlert("Theme Change", "Please restart the app to apply the theme change.", "OK");
            // Application.Current.UserAppTheme = e.Value ? OSAppTheme.Dark : OSAppTheme.Light;
        }

        private async void OnSavePreferencesClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Feature Not Implemented", "This feature is not yet implemented.", "OK");
            /* if (!App.Current.Properties.TryGetValue("UserId", out var uidObj) || uidObj is not Guid userId)
                 return;

             var selected = DietaryMap.Where(kv => kv.Value).Select(kv => kv.Key).ToList();
             await _userService.UpdateDietaryPreferencesAsync(userId, selected);
             await DisplayAlert("Saved", "Your preferences have been updated.", "OK");*/
        }

        private async void OnViewRecipeClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Feature Not Implemented", "This feature is not yet implemented.", "OK");
        }
        
        private async void OnThemeToggled(object sender, EventArgs e)
        {
            await DisplayAlert("Theme Change", "Please restart the app to apply the theme change.", "OK");
        }
    }
}
