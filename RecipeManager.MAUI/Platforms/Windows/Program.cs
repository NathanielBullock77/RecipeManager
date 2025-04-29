#define DISABLE_XAML_GENERATED_MAIN

using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RecipeManager.MAUI.WinUI.Main;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public static class Program
{
    /// <summary>
    /// Initialization of the Windows app.
    /// </summary>
    /// <param name="args"></param>
    [global::System.Runtime.InteropServices.DllImport("Microsoft.ui.xaml.dll")]
    private static extern void XamlCheckProcessRequirements();

    [global::System.STAThread]
    static void Main(string[] args)
    {
        XamlCheckProcessRequirements();
        global::WinRT.ComWrappersSupport.InitializeComWrappers();
        global::Microsoft.UI.Xaml.Application.Start((p) => {
            var app = new RecipeManager.MAUI.WinUI.App();
        });
    }
} 