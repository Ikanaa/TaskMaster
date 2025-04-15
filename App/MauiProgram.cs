using EntityFramework.Data;
using Microsoft.EntityFrameworkCore;
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
                        var mainWindow = Application.Current?.Windows.FirstOrDefault();
                        if (mainWindow != null)
                        {
                            mainWindow.Width = 800;
                            mainWindow.Height = 600;
                            mainWindow.MinimumWidth = 800;
                            mainWindow.MinimumHeight = 600;
                        }
                        else
                        {
                            Console.WriteLine("Erreur : Impossible d'accéder à la fenêtre principale.");
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

        /*builder.Services.AddDbContext<TaskmasterContext>(options =>
        {
            try
            {
                options.UseMySql(
                    "server=localhost;database=taskmaster_db;user=root;password=yourpassword;",
                    new MySqlServerVersion(new Version(10, 5, 0))
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la configuration de la base de données : {ex.Message}");
            }
        });*/


        return builder.Build();
    }
}
