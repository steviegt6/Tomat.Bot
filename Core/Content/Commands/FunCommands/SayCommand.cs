using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;

namespace TomatBot.Core.Content.Commands.FunCommands
{
    public sealed class SayCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData => new HelpCommandData("say", "Echoes the text specified, deletes the message from the user afterward.");

        public override CommandType CType => CommandType.Fun;

        [Command("say")]
        [Summary("Echoes back a message.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public Task HandleCommand(
            [Remainder]
            [Summary("The text you wish you echo.")]
            string message)
        {
            Context.Message.DeleteAsync();
            return ReplyAsync(message, embed: CreateSmallEmbed().Build());
        }
    }
}
