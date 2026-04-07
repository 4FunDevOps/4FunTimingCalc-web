// File: Infrastructure/Storage/FileStorageService.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TimingCalc.Core.Entities;
using TimingCalc.Core.Interfaces;

namespace TimingCalc.Infrastructure.Storage
{
    /// <summary>
    /// Implementazione del servizio di archiviazione che gestisce il salvataggio e il recupero 
    /// dei profili degli alberi a camme come file JSON all'interno della memoria isolata del dispositivo.
    /// </summary>
    public class FileStorageService : IStorageService
    {
        private readonly string _folderPath;

        /// <summary>
        /// Inizializza una nuova istanza di <see cref="FileStorageService"/>.
        /// Configura il percorso di salvataggio sicuro (Sandbox) e crea la cartella fisica se non esiste.
        /// </summary>
        public FileStorageService()
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            _folderPath = Path.Combine(basePath, "CamshaftDataFiles");

            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
        }

        /// <summary>
        /// Salva un'entità <see cref="CamshaftProfile"/> come file JSON fisico.
        /// Include la sanitizzazione del nome del file per prevenire errori di sistema o vulnerabilità (Path Traversal).
        /// </summary>
        /// <param name="profile">L'oggetto <see cref="CamshaftProfile"/> contenente i dati di fasatura e le note da salvare.</param>
        /// <returns>Un <see cref="Task"/> che rappresenta l'operazione asincrona di salvataggio.</returns>
        /// <exception cref="InvalidOperationException">Lanciata se il processo di scrittura sul file system fallisce.</exception>
        public Task SaveProfileAsync(CamshaftProfile profile)
        {
            try
            {
                // [SICUREZZA]: Pulisce il nome da caratteri illegali per evitare Path Traversal o Crash
                string safeName = string.Join("_", profile.Name.Split(Path.GetInvalidFileNameChars()));
                string fileName = $"{safeName}.json";
                string fullPath = Path.Combine(_folderPath, fileName);

                string jsonContent = JsonSerializer.Serialize(profile, new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(fullPath, jsonContent);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore di scrittura file: {ex.Message}");
                throw new InvalidOperationException($"Impossibile salvare il file in: {_folderPath}", ex);
            }
        }

        /// <summary>
        /// Scansiona la cartella di archiviazione, legge tutti i file JSON presenti e li converte in oggetti.
        /// Ignora automaticamente i file corrotti per garantire la stabilità dell'applicazione.
        /// </summary>
        /// <returns>Un <see cref="Task"/> contenente una <see cref="List{CamshaftProfile}"/> con tutti i profili validi trovati.</returns>
        public Task<List<CamshaftProfile>> GetAllProfilesAsync()
        {
            var profiles = new List<CamshaftProfile>();

            try
            {
                string[] files = Directory.GetFiles(_folderPath, "*.json");

                foreach (var file in files)
                {
                    try
                    {
                        // [STABILITÀ]: Se un file è corrotto, non blocca il caricamento degli altri
                        string jsonContent = File.ReadAllText(file);
                        var profile = JsonSerializer.Deserialize<CamshaftProfile>(jsonContent);
                        if (profile != null)
                        {
                            profiles.Add(profile);
                        }
                    }
                    catch (Exception parseEx)
                    {
                        Console.WriteLine($"Salto il file corrotto {file}: {parseEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore di lettura cartella: {ex.Message}");
            }

            return Task.FromResult(profiles);
        }

        /// <summary>
        /// Elimina fisicamente il file JSON associato al profilo specificato.
        /// </summary>
        /// <param name="profileName">Il nome del profilo da eliminare (verrà sanitizzato prima della ricerca).</param>
        /// <returns>Un Task che rappresenta l'operazione asincrona di eliminazione.</returns>
        /// <exception cref="InvalidOperationException">Lanciata se l'eliminazione del file fallisce per problemi di accesso o di I/O.</exception>
        public Task DeleteProfileAsync(string profileName)
        {
            try
            {
                string safeName = string.Join("_", profileName.Split(Path.GetInvalidFileNameChars()));
                string fileName = $"{safeName}.json";
                string fullPath = Path.Combine(_folderPath, fileName);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
                throw new InvalidOperationException("Could not delete the profile file.", ex);
            }
        }
    }
}