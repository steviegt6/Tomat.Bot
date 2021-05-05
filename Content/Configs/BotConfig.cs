using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Newtonsoft.Json;
using TomatBot.Content.Services;

namespace TomatBot.Content.Configs
{
    public sealed class BotConfig
    {
        public static string UserConfigsPath => $"{ConfigService.ConfigPath}/users.json";

        public static string GuildConfigsPath => $"{ConfigService.ConfigPath}/guilds.json";

        public static string GlobalConfigPath => $"{ConfigService.ConfigPath}/global.json";

        public List<UserConfig> Users { get; private set; } = null!;

        public List<GuildConfig> Guilds { get; private set; } = null!;

        public GlobalConfig Global { get; private set; } = null!;

        public BotConfig() => LoadConfigs().GetAwaiter().GetResult();

        public async Task LoadConfigs()
        {
            Users = new List<UserConfig>();
            Guilds = new List<GuildConfig>();
            Global = new GlobalConfig();

            if (File.Exists(UserConfigsPath)) 
                Users = JsonConvert.DeserializeObject<List<UserConfig>>(await File.ReadAllTextAsync(UserConfigsPath));

            if (File.Exists(GuildConfigsPath))
                Guilds = JsonConvert.DeserializeObject<List<GuildConfig>>(await File.ReadAllTextAsync(GuildConfigsPath));

            if (File.Exists(GlobalConfigPath))
                Global = JsonConvert.DeserializeObject<GlobalConfig>(await File.ReadAllTextAsync(GlobalConfigPath));

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
            File.WriteAllText(UserConfigsPath, JsonConvert.SerializeObject(Users, Formatting.Indented, ConfigService.SerializationSettings));
            File.WriteAllText(GuildConfigsPath, JsonConvert.SerializeObject(Guilds, Formatting.Indented, ConfigService.SerializationSettings));
            File.WriteAllText(GlobalConfigPath, JsonConvert.SerializeObject(Global, Formatting.Indented, ConfigService.SerializationSettings));
        }
    }
}
