// File: Core/Interfaces/ICamshaftCalculatorService.cs

using TimingCalc.Core.Entities;

namespace TimingCalc.Core.Interfaces
{
    /// <summary>
    /// Definisce i contratti metodologici per il calcolo delle specifiche geometriche e di fasatura del motore.
    /// </summary>
    public interface ICamshaftCalculatorService
    {
        /// <summary>
        /// Calcola la durata totale dell'evento di aspirazione in gradi di rotazione dell'albero motore.
        /// </summary>
        /// <param name="profile">L'entità contenente i parametri di fasatura correnti.</param>
        /// <returns>La durata dell'aspirazione espressa in gradi.</returns>
        int CalculateIntakeDuration(CamshaftProfile profile);

        /// <summary>
        /// Calcola la durata totale dell'evento di scarico in gradi di rotazione dell'albero motore.
        /// </summary>
        /// <param name="profile">L'entità contenente i parametri di fasatura correnti.</param>
        /// <returns>La durata dello scarico espressa in gradi.</returns>
        int CalculateExhaustDuration(CamshaftProfile profile);

        /// <summary>
        /// Calcola i gradi totali in cui sia la valvola di aspirazione che quella di scarico rimangono aperte simultaneamente (Incrocio).
        /// </summary>
        /// <param name="profile">L'entità contenente i parametri di fasatura correnti.</param>
        /// <returns>Il valore dell'incrocio valvole espresso in gradi.</returns>
        int CalculateOverlap(CamshaftProfile profile);

        /// <summary>
        /// Calcola l'angolo di separazione tra i lobi (LSA), ovvero la distanza in gradi tra le centerline di aspirazione e scarico.
        /// </summary>
        /// <param name="profile">L'entità contenente i parametri di fasatura correnti.</param>
        /// <returns>L'angolo di separazione dei lobi (LSA) in gradi.</returns>
        double CalculateLobeSeparationAngle(CamshaftProfile profile);

        /// <summary>
        /// Calcola la mezzeria esatta (Centerline) del lobo di aspirazione calcolata rispetto al punto morto superiore.
        /// </summary>
        /// <param name="profile">L'entità contenente i parametri di fasatura correnti.</param>
        /// <returns>La Centerline di aspirazione espressa in gradi (ATDC).</returns>
        double CalculateIntakeCenterline(CamshaftProfile profile);

        /// <summary>
        /// Calcola la mezzeria esatta (Centerline) del lobo di scarico calcolata rispetto al punto morto superiore.
        /// </summary>
        /// <param name="profile">L'entità contenente i parametri di fasatura correnti.</param>
        /// <returns>La Centerline di scarico espressa in gradi (BTDC).</returns>
        double CalculateExhaustCenterline(CamshaftProfile profile);
    }
}