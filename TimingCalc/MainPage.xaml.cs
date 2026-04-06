namespace TimingCalc;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        //// Avviamo il timer per lo splash screen
        //_ = ForceMinimumSplashScreenDuration();
    }

    //private async Task ForceMinimumSplashScreenDuration()
    //{
    //    // 1. Nascondi l'app all'avvio
    //    blazorWebView.IsVisible = false;

    //    // 2. Aspetta 2 secondi (2000 ms) in background
    //    await Task.Delay(3000);

    //    // 3. Usa il Dispatcher per assicurarti che la riattivazione avvenga 
    //    // sul thread grafico principale, evitando qualsiasi avviso di sistema o crash
    //    Dispatcher.Dispatch(() =>
    //    {
    //        blazorWebView.IsVisible = true;
    //    });
    //}
}
