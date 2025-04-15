using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureLifecycleEvents(events =>
             {
#if WINDOWS
                 events.AddWindows(windows =>
                 {
                     windows.OnLaunched((app, args) =>
                     {
                         // Accéder à la première fenêtre via Application.Current.Windows
                         var mainWindow = Application.Current?.Windows.FirstOrDefault();
                         if (mainWindow != null)
                         {
                             mainWindow.Width = 800; // Largeur de la fenêtre
                             mainWindow.Height = 600; // Hauteur de la fenêtre

                            // mainWindow.MaximumWidth = mainWindow.Width;
                            // mainWindow.MaximumHeight = mainWindow.Height;
                             mainWindow.MinimumWidth = mainWindow.Width;
                             mainWindow.MinimumHeight = mainWindow.Height;
                         }
                     });
                 });
#endif
             })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}