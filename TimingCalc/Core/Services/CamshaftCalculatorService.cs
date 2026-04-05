// File: Core/Services/CamshaftCalculatorService.cs

using TimingCalc.Core.Entities;
using TimingCalc.Core.Interfaces;

namespace TimingCalc.Core.Services
{
    /// <summary>
    /// Servizio che racchiude la logica matematica per il calcolo dei parametri di fasatura dell'albero a camme.
    /// </summary>
    public class CamshaftCalculatorService : ICamshaftCalculatorService
    {
        /// <summary>
        /// Calcola la durata totale della fase di aspirazione in gradi dell'albero motore.
        /// </summary>
        /// <param name="profile">L'entità CamshaftProfile contenente i dati di fasatura inseriti dall'utente.</param>
        /// <returns>La durata dell'aspirazione in gradi (int).</returns>
        public int CalculateIntakeDuration(CamshaftProfile profile)
        {
            return profile.IntakeOpensBtdc.GetValueOrDefault() + profile.IntakeClosesAbdc.GetValueOrDefault() + 180;
        }

        /// <summary>
        /// Calcola la durata totale della fase di scarico in gradi dell'albero motore.
        /// </summary>
        /// <param name="profile">L'entità CamshaftProfile contenente i dati di fasatura inseriti dall'utente.</param>
        /// <returns>La durata dello scarico in gradi (int).</returns>
        public int CalculateExhaustDuration(CamshaftProfile profile)
        {
            return profile.ExhaustOpensBbdc.GetValueOrDefault() + profile.ExhaustClosesAtdc.GetValueOrDefault() + 180;
        }

        /// <summary>
        /// Calcola l'incrocio valvole (Overlap), ovvero il periodo in gradi in cui le valvole di aspirazione e scarico sono aperte contemporaneamente.
        /// </summary>
        /// <param name="profile">L'entità CamshaftProfile contenente i dati di fasatura inseriti dall'utente.</param>
        /// <returns>I gradi di incrocio valvole (int).</returns>
        public int CalculateOverlap(CamshaftProfile profile)
        {
            return profile.IntakeOpensBtdc.GetValueOrDefault() + profile.ExhaustClosesAtdc.GetValueOrDefault();
        }

        /// <summary>
        /// Calcola il Lobe Separation Angle (LSA), l'angolo di separazione geometrica tra le centerline dei lobi.
        /// </summary>
        /// <param name="profile">L'entità CamshaftProfile contenente i dati di fasatura inseriti dall'utente.</param>
        /// <returns>Il LSA in gradi (double).</returns>
        public double CalculateLobeSeparationAngle(CamshaftProfile profile)
        {
            double intakeDuration = CalculateIntakeDuration(profile);
            double exhaustDuration = CalculateExhaustDuration(profile);

            double intakeCenterline = (intakeDuration / 2) - profile.IntakeOpensBtdc.GetValueOrDefault();
            double exhaustCenterline = (exhaustDuration / 2) - profile.ExhaustClosesAtdc.GetValueOrDefault();

            return (intakeCenterline + exhaustCenterline) / 2;
        }

        /// <summary>
        /// Calcola la Centerline di Aspirazione (Intake Centerline), il punto di massima alzata del lobo dopo il Punto Morto Superiore (ATDC).
        /// </summary>
        /// <param name="profile">L'entità CamshaftProfile contenente i dati di fasatura inseriti dall'utente.</param>
        /// <returns>La Centerline di aspirazione in gradi (double).</returns>
        public double CalculateIntakeCenterline(CamshaftProfile profile)
        {
            double duration = CalculateIntakeDuration(profile);
            return (duration / 2) - profile.IntakeOpensBtdc.GetValueOrDefault();
        }

        /// <summary>
        /// Calcola la Centerline di Scarico (Exhaust Centerline), il punto di massima alzata del lobo prima del Punto Morto Superiore (BTDC).
        /// </summary>
        /// <param name="profile">L'entità CamshaftProfile contenente i dati di fasatura inseriti dall'utente.</param>
        /// <returns>La Centerline di scarico in gradi (double).</returns>
        public double CalculateExhaustCenterline(CamshaftProfile profile)
        {
            double duration = CalculateExhaustDuration(profile);
            return (duration / 2) - profile.ExhaustClosesAtdc.GetValueOrDefault();
        }
    }
}