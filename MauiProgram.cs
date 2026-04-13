using Microsoft.Extensions.Logging;
using Declaraties.Services;
using Declaraties.ViewModels;
using Declaraties.Views;

namespace Declaraties
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register services and viewmodels
            builder.Services.AddSingleton<IMonthRecordRepository, MonthRecordRepository>();
            builder.Services.AddSingleton<INotesRepository, NotesRepository>();
            builder.Services.AddSingleton<MonthViewModel>();
            builder.Services.AddSingleton<MonthPage>();
            builder.Services.AddSingleton<TotalsPage>();
            builder.Services.AddSingleton<TotalsViewModel>();
            builder.Services.AddSingleton<NotesPage>();
            builder.Services.AddSingleton<NotesViewModel>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<SettingsViewModel>();




#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
