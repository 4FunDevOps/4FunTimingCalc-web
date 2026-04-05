// File: Platforms/Android/MainActivity.cs
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui;
using System;
using TimingCalc.Core.Services;

namespace TimingCalc
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
        LaunchMode = LaunchMode.SingleTop, // CRITICO: Evita cloni dell'app
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]

    // Filtro 1: Link standard web 
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "https",
        DataHost = "timingcalc.app",
        DataPathPrefix = "/share")]

    // Filtro 2: Custom URI Scheme infallibile per i test 
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "timingcalc",
        DataHost = "share")]
    public class MainActivity : MauiAppCompatActivity
    {
        /// <summary>
        /// Gestisce il caso in cui l'app viene avviata da zero.
        /// </summary>
        /// <param name="savedInstanceState">Stato precedente salvato (può essere null).</param>
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ProcessIntent(Intent);
        }

        /// <summary>
        /// Gestisce il caso in cui l'app era già in background e viene "svegliata" dal link.
        /// </summary>
        /// <param name="intent">L'intent di Android (può essere null).</param>
        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);
            ProcessIntent(intent);
        }

        /// <summary>
        /// Estrae l'URI dall'Intent e lo passa al servizio Blazor in modo sicuro.
        /// </summary>
        /// <param name="intent">L'oggetto Intent fornito dal sistema operativo Android.</param>
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
                        deepLinkState.PendingUri = uri;
                        deepLinkState.NotifyLinkReceived(uri);
                    }
                }
            }
        }
    }
}