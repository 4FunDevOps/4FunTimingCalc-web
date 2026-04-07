// File: MauiProgram.cs

#pragma warning disable CA1416

using Microsoft.Extensions.Logging;
using TimingCalc.Core.Interfaces;
using TimingCalc.Core.Services;
using TimingCalc.Infrastructure.Storage;
using CommunityToolkit.Maui;

namespace TimingCalc
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // Iniezione delle dipendenze per la Clean Architecture in ambiente MAUI
            // Il servizio di storage ora punta al file system fisico del dispositivo
            builder.Services.AddScoped<ICamshaftCalculatorService, CamshaftCalculatorService>();
            builder.Services.AddScoped<IStorageService, FileStorageService>();
            builder.Services.AddSingleton<TimingCalc.Core.Services.DeepLinkStateService>();

            return builder.Build();
        }
    }
}