using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using TomatBot.Core.Content.Activities;
using TomatBot.Core.Exceptions.IOExceptions;
using TomatBot.Core.Logging;
using Timer = System.Timers.Timer;

namespace TomatBot.Core
{
    public class BotStartup
    {
        private static readonly CancellationTokenSource StopTokenSource = new();
        private static readonly CancellationToken StopToken = StopTokenSource.Token;
        private static bool _shuttingDown;
        
        // TODO: Transfer to guild config system, guild-configurable prefixes with "tomat!" as a fallback (mentions also work)
        public static string Prefix => "tomat!";

        public static IServiceCollection Collection { get; private set; } = null!;

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

            // Create a new DiscordSocketClient and hook a logging method
            DiscordSocketClient client = new(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                ExclusiveBulkDelete = false, // eventually purge logging
                DefaultRetryMode = RetryMode.RetryRatelimit,
                AlwaysDownloadUsers = true,
                ConnectionTimeout = 30 * 1000,
                MessageCacheSize = 50 // increase if bot is added to more servers?
            });
            client.Log += Logger.TaskLog;

            // Handles standard SIGTERM (-15) Signals
            AppDomain.CurrentDomain.ProcessExit += (_, _) =>
            {
                if (!_shuttingDown)
                    ShutdownBotAsync().GetAwaiter().GetResult();
            };

            SetupSingletons(client);

            // Login and start
            await client.LoginAsync(TokenType.Bot,

                // Grab token from a token.txt file
                // The file is copied over when compiled
                // Ignored by git
                // Verified that the file exists at the very beginning of the start-up process
                await File.ReadAllTextAsync("token.txt"));

            // Actually start the client
            await client.StartAsync();

            client.Ready += async () =>
                            {
                                await CheckForRestart(client);
                                await ModifyBotStatus(client);
                            };

            // Block until the program is closed
            try { await Task.Delay(-1, StopToken); }
            catch (TaskCanceledException) { /* ignore */ }
        }

        private static void SetupSingletons(DiscordSocketClient client)
        {
            Collection = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    CaseSensitiveCommands = false,
                    IgnoreExtraArgs = false,
                    DefaultRunMode = RunMode.Async
                }))
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
                catch (Exception) { /* ignore */ }
                finally
                {
                    File.Delete("Restarted.txt");
                }
            }
        }

        private static async Task ModifyBotStatus(BaseSocketClient client)
        {
            // Set activity and status for the bot
            await client.SetActivityAsync(new StatisticsActivity(Client));
            new Timer(10000)
            {
                AutoReset = true,
                Enabled = true,
            }.Elapsed += async (_, _)
                             => await Client.SetActivityAsync(new StatisticsActivity(Client));
            // Set status to DND
            await client.SetStatusAsync(UserStatus.DoNotDisturb);
        }

        internal static async Task ShutdownBotAsync()
        {
            _shuttingDown = true;
            
            StopTokenSource.Cancel();
            await Provider.DisposeAsync();
            await Task.Run(() => Client?.StopAsync());
        }
    }
}