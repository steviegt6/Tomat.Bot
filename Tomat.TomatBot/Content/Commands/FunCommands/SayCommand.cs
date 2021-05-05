using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Tomat.CommandFramework;
using Tomat.CommandFramework.HelpSystem;
using Tomat.Conveniency.Utilities;

namespace Tomat.TomatBot.Content.Commands.FunCommands
{
    public sealed class SayCommand : BaseCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData =>
            new("say", "Echoes the text specified, deletes the message from the user afterward.");

        public override CommandType CType => CommandType.Fun;

        public override string Parameters => "<message>";

        [Command("say")]
        [Summary("Echoes back a message.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task HandleCommand(
            [Remainder] [Summary("The text you wish you echo.")]
            string message)
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(message, embed: EmbedHelper.CreateSmallEmbed(Context.User).Build(),
                allowedMentions: AllowedMentions.None);
        }
    }
}