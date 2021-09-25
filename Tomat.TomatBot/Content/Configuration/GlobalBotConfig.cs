#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Discord.WebSocket;
using Newtonsoft.Json;
using Tomat.TomatBot.Core.Bot;
using Tomat.TomatBot.Core.Configuration;

namespace Tomat.TomatBot.Content.Configuration
{
    public class GlobalBotConfig : IConfigFile
    {
        public DiscordBot Bot { get; }

        public string SavePath => Path.Combine("Data", "GlobalData.json");

        // Not populated on config load.
        // public Dictionary<ulong, UserConfig> UserConfigs { get; } = new();

        // Populated on config load.
        public Dictionary<ulong, GuildData> GuildConfigs { get; } = new();

        public GlobalData Data { get; set; } = null!;

        public GlobalBotConfig(DiscordBot bot)
        {
            Bot = bot;
        }

        public async Task LoadConfig()
        {
            // Pre-load guild configs.
            foreach (SocketGuild guild in Bot.DiscordClient.Guilds)
            {
                if (GuildConfigs.ContainsKey(guild.Id))
                    continue;

                if (File.Exists(GetGuildLocation(guild.Id)))
                    GuildConfigs[guild.Id] = JsonConvert.DeserializeObject<GuildData>(
                        await File.ReadAllTextAsync(GetGuildLocation(guild.Id))
                    );
                else
                    GuildConfigs[guild.Id] = new GuildData
                    {
                        AssociatedId = guild.Id
                    };
            }

            Data = File.Exists(SavePath)
                ? JsonConvert.DeserializeObject<GlobalData>(await File.ReadAllTextAsync(SavePath))
                : new GlobalData();

            await SaveConfig();
        }

        public async Task SaveConfig()
        {
            foreach (GuildData gConfig in GuildConfigs.Values)
                await SerializeObject(gConfig, GetGuildLocation(gConfig.AssociatedId));

            await SerializeObject(Data, SavePath);
        }

        public string GetGuildLocation(ulong id) => Path.Combine("Data", "Guilds", id + ".json");

        public async Task SerializeObject(object value, string savePath)
        {
            Directory.GetParent(savePath)?.Create();
            await File.WriteAllTextAsync(savePath, JsonConvert.SerializeObject(value, Formatting.Indented));
        }
    }
}