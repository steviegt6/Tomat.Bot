using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TomatBot.Core.Content.Services;

namespace TomatBot.Core.Content
{
    public sealed class BotConfig
    {
        public static string UserConfigsDirectory => $"{ConfigService.ConfigPath}/Users";

        public static string GuildConfigsDirectory => $"{ConfigService.ConfigPath}/Users";

        public List<UserConfig> Users { get; private set; } = null!;

        public List<GuildConfig> Guilds { get; private set; } = null!;

        public BotConfig(List<UserConfig> users, List<GuildConfig> guilds)
        {
            Users = users;
            Guilds = guilds;
        }

        public BotConfig() => LoadConfigs().GetAwaiter().GetResult();

        public async Task LoadConfigs()
        {
            Directory.CreateDirectory(UserConfigsDirectory);
            Directory.CreateDirectory(GuildConfigsDirectory);

            Users = new List<UserConfig>();
            Guilds = new List<GuildConfig>();

            foreach (var file in Directory.GetFiles(UserConfigsDirectory, "*.json", SearchOption.TopDirectoryOnly))
                Users.Add(JsonConvert.DeserializeObject<UserConfig>(await File.ReadAllTextAsync(file)));

            foreach (var file in Directory.GetFiles(GuildConfigsDirectory, "*.json", SearchOption.TopDirectoryOnly))
                Guilds.Add(JsonConvert.DeserializeObject<GuildConfig>(await File.ReadAllTextAsync(file)));
        }

        public async Task SaveConfigs()
        {
            foreach (UserConfig user in Users)
                await File.WriteAllTextAsync($"{UserConfigsDirectory}/{user.AssociatedId}.json", JsonConvert.SerializeObject(user, Formatting.Indented, ConfigService.SerializationSettings));

            foreach (GuildConfig guild in Guilds)
                await File.WriteAllTextAsync($"{GuildConfigsDirectory}/{guild.AssociatedId}.json", JsonConvert.SerializeObject(guild, Formatting.Indented, ConfigService.SerializationSettings));
        }
    }
}
