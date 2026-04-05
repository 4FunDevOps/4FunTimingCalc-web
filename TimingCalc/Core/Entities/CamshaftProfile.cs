// File: Core/Entities/CamshaftProfile.cs
using System;

namespace TimingCalc.Core.Entities
{
    /// <summary>
    /// Rappresenta il profilo completo di un albero a camme, includendo i dati di fasatura e le note tecniche.
    /// </summary>
    public class CamshaftProfile
    {
        /// <summary>
        /// Identificatore univoco del profilo, generato automaticamente alla creazione.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Nome o codice identificativo dell'albero a camme (es. Race Spec 1).
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Anticipo apertura aspirazione (BTDC). Nullable per permettere campo vuoto in UI.
        /// </summary>
        public int? IntakeOpensBtdc { get; set; }

        /// <summary>
        /// Posticipo chiusura aspirazione (ABDC). Nullable per permettere campo vuoto in UI.
        /// </summary>
        public int? IntakeClosesAbdc { get; set; }

        /// <summary>
        /// Anticipo apertura scarico (BBDC). Nullable per permettere campo vuoto in UI.
        /// </summary>
        public int? ExhaustOpensBbdc { get; set; }

        /// <summary>
        /// Ritardo chiusura scarico (ATDC). Nullable per permettere campo vuoto in UI.
        /// </summary>
        public int? ExhaustClosesAtdc { get; set; }

        /// <summary>
        /// Specifiche tecniche aggiuntive.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// Data e ora esatta in cui il profilo è stato elaborato.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}