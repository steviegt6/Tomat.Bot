using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace TomatBot.Core
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            _client = client;
            _commands = commands;
        }

        internal async Task InstallCommandsAsync()
        {
            // Hook a method to the MessageReceived event, allowing us to detect and respond to messages
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        private async Task HandleCommandAsync(SocketMessage message)
        {
            if (message is SocketUserMessage userMessage)
            {
                int argPos = 0;

                // If the message doesn't start with our prefix *or* has no mention as the prefix, ignore. Ignored if the message author is a bot as well
                if (!userMessage.HasStringPrefix(BotStartup.Prefix, ref argPos) &&
                    !userMessage.HasMentionPrefix(_client.CurrentUser, ref argPos) || userMessage.Author.IsBot)
                    return;

                await _commands.ExecuteAsync(new SocketCommandContext(_client, userMessage), argPos, null);
            }
        }
    }
}