using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;

namespace TomatBot.Core.Content.Commands.FunCommands
{
    public sealed class SayCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData => new("say", "Echoes the text specified, deletes the message from the user afterward.");

        public override CommandType CType => CommandType.Fun;

        public override string Parameters => "<message>";

        [Command("say")]
        [Summary("Echoes back a message.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task HandleCommand(
            [Remainder]
            [Summary("The text you wish you echo.")]
            string message)
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(message, embed: CreateSmallEmbed().Build(), allowedMentions:AllowedMentions.None);
        }

        public async Task HandleCommand(
            [Summary("The channel in which the bot should echo the text")] SocketTextChannel channel, 
            [Remainder] [Summary("The text")] string text)
        {
            await Context.Message.DeleteAsync();
            await channel.SendMessageAsync(text, embed: CreateSmallEmbed().Build(), allowedMentions:AllowedMentions.None);
        }
        
    }
}
