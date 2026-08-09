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
    public class FileStorageService : IStorageService
    {
        private readonly string _folderPath;

        public FileStorageService()
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            _folderPath = Path.Combine(basePath, "CamshaftDataFiles");

            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
        }

        public Task SaveProfileAsync(CamshaftProfile profile)
        {
            try
            {
                // [CORREZIONE CS0019 e CS0029]: Verifica stringa vuota e assegna stringa GUID
                if (string.IsNullOrWhiteSpace(profile.Id))
                {
                    profile.Id = Guid.NewGuid().ToString();
                }

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