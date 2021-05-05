using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Tomat.Conveniency.Activities;
using Tomat.Logging;
using Tomat.ServiceFramework;
using TomatBot.Content.Services;
using TomatBot.Exceptions.IOExceptions;
using Timer = System.Timers.Timer;

namespace TomatBot
{
    public static class BotStartup
    {
        private static readonly CancellationTokenSource StopTokenSource = new();
        private static readonly CancellationToken StopToken = StopTokenSource.Token;
        private static bool _shuttingDown;

        public static string DefaultPrefix => Debugger.IsAttached
            ? "edge!"
            : "tomat!";

        public static IServiceCollection? Collection { get; private set; }

        public static ServiceProvider Provider => Collection.BuildServiceProvider();

        public static DiscordSocketClient Client => Provider.GetRequiredService<DiscordSocketClient>();

        public static TimeSpan UpTime => DateTimeOffset.Now - Process.GetCurrentProcess().StartTime;

        /// <summary>
        ///     Starts up our Discord bot.
        /// </summary>
        /// <returns>An indefinitely delayed task to keep the program alive.</returns>
        /// <exception cref="TokenFileMissingException">A <c>token.txt</c> file was not found in the same directory as the program.</exception>
        internal static async Task StartBotAsync()
        {
            if (!File.Exists("token.txt"))
                throw new TokenFileMissingException();

            // Handles standard SIGTERM (-15) Signals
            AppDomain.CurrentDomain.ProcessExit += (_, _) =>
            {
                Provider.GetRequiredService<ConfigService>().Config
                    .SaveConfigs();
                if (!_shuttingDown)
                    ShutdownBotAsync().GetAwaiter().GetResult();
            };

            SetupSingletons();
            Client.Log += Logger.TaskLog;

            // Login and start
            await Client.LoginAsync(TokenType.Bot,

                // Grab token from a token.txt file
                // The file is copied over when compiled
                // Ignored by git
                // Verified that the file exists at the very beginning of the start-up process
                await File.ReadAllTextAsync("token.txt"));

            // Actually start the client
            await Client.StartAsync();

            Client.Ready += async () =>
            {
                if (Collection != null) 
                    await ServiceRegistry.InitializeServicesAsync(Collection, Provider);
                await CheckForRestart(Client);
                await ModifyBotStatus(Client);
                Provider.GetRequiredService<ConfigService>().Config.PopulateGuildsList();

                // Auto-save all configs every hour
                new Timer(60 * 60 * 1000)
                {
                    AutoReset = true,
                    Enabled = true
                }.Elapsed += (_, _)
                    => Provider.GetRequiredService<ConfigService>().Config.SaveConfigs();
            };

            // Block until the program is closed
            try
            {
                await Task.Delay(-1, StopToken);
            }
            catch (TaskCanceledException)
            {
                /* ignore */
            }
        }

        private static void SetupSingletons()
        {
            Collection = new ServiceCollection()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Info,
                    ExclusiveBulkDelete = false, // eventually purge logging
                    DefaultRetryMode = RetryMode.RetryRatelimit,
                    AlwaysDownloadUsers = true,
                    ConnectionTimeout = 30 * 1000,
                    MessageCacheSize = 50, // increase if bot is added to more servers?
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    CaseSensitiveCommands = false,
                    IgnoreExtraArgs = false,
                    DefaultRunMode = RunMode.Async
                }));

            Collection.AddSingleton(new ConfigService(Provider))
                .AddSingleton(new CommandHandler());
        }

        private static async Task CheckForRestart(BaseSocketClient client)
        {
            if (File.Exists("Restarted.txt"))
            {
                try
                {
                    string[] ids = File.ReadAllTextAsync("Restarted.txt").Result.Split(' ');

                    if (ids.Length < 2)
                        return;

                    Embed? embed = new EmbedBuilder
                    {
                        Title = "Bot restarted successfully",
                        Color = Color.Green
                    }.Build();
                    ulong guildId = ulong.Parse(ids[0]);
                    ulong channelId = ulong.Parse(ids[1]);
                    SocketTextChannel channel = (client.GetGuild(guildId).GetChannel(channelId) as SocketTextChannel)!;

                    await channel.SendMessageAsync(embed: embed);
                }
                catch (Exception)
                {
                    /* ignore */
                }
                finally
                {
                    File.Delete("Restarted.txt");
                }
            }
        }

        private static async Task ModifyBotStatus(BaseSocketClient client)
        {
            // Set activity
            await client.SetActivityAsync(new StatisticsActivity(client.Guilds));

            // Begin refreshing the activity
            new Timer(10 * 1000)
            {
                AutoReset = true,
                Enabled = true
            }.Elapsed += async (_, _)
                => await Client.SetActivityAsync(new StatisticsActivity(client.Guilds));

            // Set status to DND
            await client.SetStatusAsync(UserStatus.DoNotDisturb);
        }

        internal static async Task ShutdownBotAsync()
        {
            _shuttingDown = true;

            StopTokenSource.Cancel();
            await Provider.DisposeAsync();
            await Task.Run(() => Client.StopAsync());
        }

        public static string GetGuildPrefix(IGuild? guild)
        {
            try
            {
                string value = Provider.GetRequiredService<ConfigService>().Config.Guilds
                    .First(x => x.associatedId == guild?.Id).guildPrefix;

                if (value == "tomat!" && Debugger.IsAttached)
                    return "edge!";

                return value;
            }
            catch
            {
                return "";
            }
        }
    }
}