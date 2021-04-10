using System;
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
        private static CancellationTokenSource _stopTokenSource = new ();
        private static CancellationToken _stopToken = _stopTokenSource.Token;
        private static bool _shuttingDown;
        
        // TODO: Transfer to guild config system, guild-configurable prefixes with "tomat!" as a fallback (mentions also work)
        public static string Prefix => "tomat!";
        
        // TODO: Get this from Service Collection to actually use DependencyInjection
        public static DiscordSocketClient? Client { get; private set; }

        /// <summary>
        ///     Starts up our Discord bot.
        /// </summary>
        /// <returns>An indefinitely delayed task to keep the program alive.</returns>
        /// <exception cref="TokenFileMissingException">A <c>token.txt</c> file was not found in the same directory as the program.</exception>
        internal async Task StartBotAsync()
        {
            if (!File.Exists("token.txt"))
                throw new TokenFileMissingException();

            // Create a new DiscordSocketClient and hook a logging method
            Client = new DiscordSocketClient();
            Client.Log += Logger.TaskLog;

            // Handles standard SIGTERM (-15) Signals
            AppDomain.CurrentDomain.ProcessExit += (_, _) =>
            {
                if (!_shuttingDown)
                    ShutdownBotAsync().GetAwaiter().GetResult();
            };
            
            // TODO: Save these?
            new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(new CommandService())
                .BuildServiceProvider();

            // TODO: Why not add this to the service collection? The service collection automatically calls the constructor. Events have to be added in a initialize method
            await new CommandHandler(Client, new CommandService())
                .InstallCommandsAsync();

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
                if (File.Exists("Restarted.txt"))
                {
                    try
                    {
                        var ids = File.ReadAllTextAsync("Restarted.txt").Result.Split(' ');

                        var embed = new EmbedBuilder
                        {
                            Title = "Bot restarted successfully",
                            Color = Color.Green
                        }.Build();

                        await (Client.GetGuild(ulong.Parse(ids[0])).GetChannel(ulong.Parse(ids[1])) as SocketTextChannel)!.SendMessageAsync(embed:embed);
                    }
                    catch (Exception e) { /* ignore */ }
                    finally
                    {
                        File.Delete("Restarted.txt");
                    }
                }
                
                // Set activity and status for the bot
                await Client.SetActivityAsync(new StatisticsActivity(Client));
                new Timer(10000)
                {
                    AutoReset = true,
                    Enabled = true,
                }.Elapsed += async (a, b)
                    => await Client.SetActivityAsync(new StatisticsActivity(Client));
                // Set status to DND
                await Client.SetStatusAsync(UserStatus.DoNotDisturb);
            };

            // Block until the program is closed
            try { await Task.Delay(-1, _stopToken); }
            catch (TaskCanceledException e) { /* ignore */ }
        }

        internal async Task ShutdownBotAsync()
        {
            _shuttingDown = true;
            
            _stopTokenSource.Cancel();

            // TODO: Add this at some point, you currently don't save the service collection anywhere
            // ServiceCollection.DisposeAsync(); 

            await Task.Run(() => Client?.StopAsync());
        }
    }
}