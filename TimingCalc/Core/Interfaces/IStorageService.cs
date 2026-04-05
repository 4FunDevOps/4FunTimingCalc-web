// File: Core/Interfaces/IStorageService.cs

using System.Collections.Generic;
using System.Threading.Tasks;
using TimingCalc.Core.Entities;

namespace TimingCalc.Core.Interfaces
{
    /// <summary>
    /// Definisce le operazioni per rendere persistenti i profili salvati.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Salva in modo asincrono un nuovo profilo o aggiorna uno esistente.
        /// </summary>
        /// <param name="profile">Il profilo da salvare.</param>
        /// <returns>Task asincrono.</returns>
        Task SaveProfileAsync(CamshaftProfile profile);

        /// <summary>
        /// Recupera in modo asincrono tutti i profili salvati.
        /// </summary>
        /// <returns>Lista dei profili disponibili.</returns>
        Task<List<CamshaftProfile>> GetAllProfilesAsync();
    }
}