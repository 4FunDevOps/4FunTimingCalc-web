#pragma warning disable CA1416

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui;
using System;
using TimingCalc.Core.Services;

namespace TimingCalc
{
    [Activity(Theme = "@style/Maui.SplashTheme",
              MainLauncher = true,
              LaunchMode = LaunchMode.SingleTop, // CRITICO: Evita cloni dell'app
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]

    // Filtro 1: Link standard web (Opzionale, utile per il futuro)
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "https",
        DataHost = "4fundevops.github.io",
        DataPathPrefix = "/4FunTimingCalc-web")]

    // Filtro 2: Custom URI Scheme (Quello che stiamo usando ora tramite JavaScript)
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "timingcalc",
        DataHost = "share")]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ProcessIntent(Intent);
        }

        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);
            ProcessIntent(intent);
        }

        private void ProcessIntent(Intent? intent)
        {
            if (intent?.Action == Intent.ActionView && intent.Data != null)
            {
                string? uriString = intent.Data.ToString();

                if (!string.IsNullOrEmpty(uriString))
                {
                    var uri = new Uri(uriString);
                    var deepLinkState = IPlatformApplication.Current?.Services.GetService<DeepLinkStateService>();

                    if (deepLinkState != null)
                    {
                        // Salva l'URI e avvisa la UI (Blazor)
                        deepLinkState.PendingUri = uri;
                        deepLinkState.NotifyLinkReceived(uri);
                    }
                }
            }
        }
    }
}