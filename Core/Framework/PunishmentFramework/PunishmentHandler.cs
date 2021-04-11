using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace TomatBot.Core.Framework.PunishmentFramework
{
    public sealed class PunishmentHandler
    {
        public const string PunishmentsPath = "../punishments.json";

        public List<Punishment> Punishments
        {
            get => _punishments;

            set
            {
                _punishments = value;
                SaveAndCheckPunishments();
            }
        }

        private List<Punishment> _punishments = new();

        public PunishmentHandler()
        {
            Punishments = new List<Punishment>();

            if (!File.Exists(PunishmentsPath))
                SaveAndCheckPunishments();

            Punishments = JsonConvert.DeserializeObject<List<Punishment>>(File.ReadAllText(PunishmentsPath));

            // Hopefully make everything automatically save?
            _ = new Timer(_ => SaveAndCheckPunishments(), 
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(30));
        }

        public void SaveAndCheckPunishments()
        {
            if (File.Exists(PunishmentsPath))
                File.Copy(PunishmentsPath, PunishmentsPath + ".bak", true);

            using StreamWriter stream = File.CreateText(PunishmentsPath);
            JsonSerializer serializer = new();
            serializer.Serialize(stream, Punishments);

            foreach (Punishment punishment in Punishments.Where(punishment => punishment.IsTemporary && punishment.ExpirationTime.HasValue && punishment.ExpirationTime.Value >= DateTime.Now))
                punishment.OnExpiration();
        }
    }
}
