using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Newtonsoft.Json;
using TomatBot.Core.Content.Services;

namespace TomatBot.Core.Content.Configs
{
    public sealed class BotConfig
    {
        public static string UserConfigsPath => $"{ConfigService.ConfigPath}/users.json";

        public static string GuildConfigsPath => $"{ConfigService.ConfigPath}/guilds.json";

        public List<UserConfig> Users { get; private set; } = null!;

        public List<GuildConfig> Guilds { get; private set; } = null!;

        public BotConfig() => LoadConfigs().GetAwaiter().GetResult();

        public async Task LoadConfigs()
        {
            Users = new List<UserConfig>();
            Guilds = new List<GuildConfig>();

            if (File.Exists(UserConfigsPath)) 
                Users = JsonConvert.DeserializeObject<List<UserConfig>>(await File.ReadAllTextAsync(UserConfigsPath));

            if (File.Exists(GuildConfigsPath))
                Guilds = JsonConvert.DeserializeObject<List<GuildConfig>>(await File.ReadAllTextAsync(GuildConfigsPath));

            SaveConfigs();
        }

        public void PopulateGuildsList()
        {
            foreach (SocketGuild guild in BotStartup.Client.Guilds)
            {
                if (Guilds.Any(x => x.associatedId == guild.Id))
                    continue;
                
                Guilds.Add(new GuildConfig { associatedId = guild.Id });
            }

            SaveConfigs();
        }

        public void SaveConfigs()
        {
            File.WriteAllTextAsync(UserConfigsPath, JsonConvert.SerializeObject(Users, Formatting.Indented, ConfigService.SerializationSettings));
            File.WriteAllTextAsync(GuildConfigsPath, JsonConvert.SerializeObject(Guilds, Formatting.Indented, ConfigService.SerializationSettings));
        }
    }
}
