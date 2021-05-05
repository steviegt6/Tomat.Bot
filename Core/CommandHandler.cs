using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using TomatBot.Core.Content.Embeds;
using TomatBot.Core.Utilities;

namespace TomatBot.Core
{
    public class CommandHandler
    {
        private DiscordSocketClient Client { get; }

        private CommandService Commands { get; }

        public CommandHandler()
        {
            Client = BotStartup.Client;
            Commands = BotStartup.Provider.GetRequiredService<CommandService>();
            _ = InstallCommandsAsync();
        }

        internal async Task InstallCommandsAsync()
        {
            // Hook a method to the MessageReceived event, allowing us to detect and respond to messages
            Client.MessageReceived += HandleCommandAsync;
            Commands.CommandExecuted += HandleError;

            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        private async Task HandleCommandAsync(SocketMessage message)
        {
            if (message.ValidateMessageMention(out CommandUtilities.InvalidMessageReason invalidReason, out int argPos,
                Client))
                await Commands.ExecuteAsync(new SocketCommandContext(Client, message as SocketUserMessage), argPos,
                    null);

            if (CommandUtilities.IsFatalReason(invalidReason))
            {
                BaseEmbed embed = new(message.Author)
                {
                    Title = $"Validation error encountered: {invalidReason}",
                    Description =
                        "This likely isn't much of an issue. If you believe it is, report to the developers immediately!"
                };

                await message.Channel.SendMessageAsync(embed: embed.Build());
            }
        }

        private static async Task HandleError(Optional<CommandInfo> info, ICommandContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                BaseEmbed embed = new(context.User)
                {
                    Title = $"Error encountered: {result.Error}",
                    Description = result.ErrorReason
                };

                await context.Channel.SendMessageAsync(embed: embed.Build());
            }
        }
    }
}