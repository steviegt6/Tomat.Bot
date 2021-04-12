using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using TomatBot.Core.Content.Embeds;

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
            if (message is not SocketUserMessage msg || message.Author.IsBot || message.Author.IsWebhook)
                return;

            int argPos = 0;
            if (msg.HasStringPrefix(BotStartup.DefaultPrefix, ref argPos) 
                || msg.HasMentionPrefix(Client.CurrentUser, ref argPos) 
                || msg.Channel is SocketGuildChannel guildChannel && msg.HasStringPrefix(BotStartup.GetGuildPrefix(guildChannel.Guild), ref argPos)) 
                await Commands.ExecuteAsync(new SocketCommandContext(Client, msg), argPos, null);
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