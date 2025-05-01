using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using MauiApp = Microsoft.Maui.Controls.Application;


// note for self reference: https://learn.microsoft.com/en-us/dotnet/maui/user-interface/theming?view=net-maui-9.0

namespace RecipeManager.MAUI.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();


        }
        // this might be super buggy..
        /* void OnThemeToggled(object sender, ToggledEventArgs e)
         {
             var merged = MauiApp.Current.Resources.MergedDictionaries;
             merged.Clear();
             if (e.Value)
                 merged.Add(new DarkTheme());
             else
                 merged.Add(new LightTheme());
         }
     }*/
        }
    }
