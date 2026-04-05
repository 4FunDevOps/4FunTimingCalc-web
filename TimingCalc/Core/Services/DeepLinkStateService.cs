// File: Core/Services/DeepLinkStateService.cs
using System;

namespace TimingCalc.Core.Services
{
    /// <summary>
    /// Servizio Singleton che funge da ponte tra il sistema operativo (Android) 
    /// e l'interfaccia utente (Blazor) per la gestione dei Deep Link in ingresso.
    /// </summary>
    public class DeepLinkStateService
    {
        /// <summary>
        /// Evento scatenato quando un nuovo link viene intercettato dal sistema operativo
        /// mentre l'applicazione è già aperta in background. Può essere null se nessuno è in ascolto.
        /// </summary>
        public event Action<Uri>? OnLinkReceived;

        /// <summary>
        /// Memorizza temporaneamente l'URI se l'app viene avviata da zero (Cold Start), 
        /// conservandolo finché l'interfaccia Blazor non è pronta a leggerlo.
        /// </summary>
        public Uri? PendingUri { get; set; }

        /// <summary>
        /// Metodo invocato dalla classe nativa Android (MainActivity) per notificare 
        /// al livello Blazor che è arrivato un nuovo pacchetto dati tramite URL.
        /// </summary>
        /// <param name="uri">L'indirizzo URI completo intercettato dal sistema operativo.</param>
        public void NotifyLinkReceived(Uri uri)
        {
            OnLinkReceived?.Invoke(uri);
        }
    }
}