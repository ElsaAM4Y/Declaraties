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
                })
                .ConfigureMauiHandlers(handlers =>
                {
#if ANDROID
                    //
                    // ⭐ LABEL FIX — Disable system font scaling
                    //
                    Microsoft.Maui.Handlers.LabelHandler.Mapper.AppendToMapping("IgnoreFontScaling", (handler, view) =>
                    {
                        if (handler.PlatformView is not null && view is Label lbl)
                        {
                            handler.PlatformView.SetTextSize(
                                Android.Util.ComplexUnitType.Sp,
                                (float)lbl.FontSize
                            );

                            handler.PlatformView.SetIncludeFontPadding(false);

                            handler.PlatformView.SetAutoSizeTextTypeWithDefaults(
                                Android.Widget.AutoSizeTextType.None
                            );
                        }
                    });

                    //
                    // ⭐ PICKER FIX — Perfect vertical alignment
                    //
                    Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("FixVerticalAlignment", (handler, view) =>
                    {
                        if (handler.PlatformView is not null)
                        {
                            // Reset default Android padding
                            handler.PlatformView.SetPadding(0, 0, 0, 0);

                            // Add clean, centered padding
                            int padding = (int)(8 * handler.PlatformView.Resources.DisplayMetrics.Density);
                            handler.PlatformView.SetPadding(padding, padding, padding, padding);

                            // Remove weird Android baseline offset
                            handler.PlatformView.SetIncludeFontPadding(false);
                        }
                    });
#endif
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
