using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using TomatBot.Core.Content.Services;
using TomatBot.Core.Logging;

namespace TomatBot.Core.Content.Configs
{
    public sealed class BotConfig
    {
        public static string UserConfigsDirectory => $"{ConfigService.ConfigPath}/Users";

        public static string GuildConfigsDirectory => $"{ConfigService.ConfigPath}/Guilds";

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

            foreach (string? file in Directory.GetFiles(UserConfigsDirectory, "*.json", SearchOption.TopDirectoryOnly))
            {
                UserConfig config = JsonConvert.DeserializeObject<UserConfig>(await File.ReadAllTextAsync(file));
                config.AssociatedId = ulong.Parse(Path.GetFileNameWithoutExtension(file));
                Users.Add(config);
            }

            foreach (string? file in Directory.GetFiles(GuildConfigsDirectory, "*.json", SearchOption.TopDirectoryOnly))
            {
                GuildConfig config = JsonConvert.DeserializeObject<GuildConfig>(await File.ReadAllTextAsync(file));
                config.AssociatedId = ulong.Parse(Path.GetFileNameWithoutExtension(file));
                Guilds.Add(config);
            }
        }

        public async Task CreateMissingConfigs()
        {
            foreach (SocketGuild guild in BotStartup.Client.Guilds)
            {
                await guild.DownloadUsersAsync();

                foreach (SocketGuildUser user in guild.Users)
                {
                    if (Users.Any(config => user.Id == config.AssociatedId))
                        break;

                    Users.Add(new UserConfig(user.Id));
                }

                if (Guilds.Any(config => guild.Id == config.AssociatedId))
                    break;

                Guilds.Add(new GuildConfig(guild.Id));
            }

            await LoggerService.TaskLog(new LogMessage(LogSeverity.Debug, "Service",
                "Successfully generated any needed configs, writing configs to files..."));

            int users = 0;
            int guilds = 0;

            foreach (UserConfig user in Users)
            {
                await File.WriteAllTextAsync($"{UserConfigsDirectory}/{user.AssociatedId}.json", JsonConvert.SerializeObject(user, Formatting.Indented, ConfigService.SerializationSettings));
                users++;
            }

            foreach (GuildConfig guild in Guilds)
            {
                await File.WriteAllTextAsync($"{GuildConfigsDirectory}/{guild.AssociatedId}.json",
                    JsonConvert.SerializeObject(guild, Formatting.Indented, ConfigService.SerializationSettings));
                guilds++;
            }

            await LoggerService.TaskLog(new LogMessage(LogSeverity.Debug, "Service",
                $"Saved {users} user configs."));
            await LoggerService.TaskLog(new LogMessage(LogSeverity.Debug, "Service",
                $"Saved {guilds} guild configs."));
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
