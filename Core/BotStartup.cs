using System.IO;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using TomatBot.Core.Content.Activities;
using TomatBot.Core.Exceptions.IOExceptions;
using TomatBot.Core.Logging;

namespace TomatBot.Core
{
    public static class BotStartup
    {
        public static string Prefix => "tomat!"; // todo: server configs
        
        public static DiscordSocketClient? Client { get; private set; }

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
            Client = new DiscordSocketClient();
            Client.Log += Logger.TaskLog;

            new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(new CommandService())
                .BuildServiceProvider();

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

            // Block until the program is closed
            await Task.Delay(-1);
        }
    }
}