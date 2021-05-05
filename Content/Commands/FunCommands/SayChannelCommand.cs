using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Tomat.CommandFramework;
using Tomat.CommandFramework.HelpSystem;
using Tomat.Conveniency.Utilities;

namespace Tomat.TomatBot.Content.Commands.FunCommands
{
    public sealed class SayChannelCommand : BaseCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData =>
            new("saychannel",
                "Echoes the text specified in the given channel, deletes the message from the user afterward.");

        public override CommandType CType => CommandType.Fun;

        public override string Parameters => "<channel> <message>";

        [Command("saychannel")]
        [Alias("saych", "chsay", "channelsay")]
        [Summary("Echoes back a message in the specified channel.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task HandleCommand(
            [Summary("The channel you want the text to be displayed in")]
            SocketTextChannel channel,

            [Remainder] [Summary("The text you wish you echo.")]
            string message)
        {
            if (!(Context.User as SocketGuildUser)!.GetPermissions(channel).SendMessages)
            {
                await ReplyAsync(
                    embed: EmbedHelper.ErrorEmbed("You don't have permissions to send messages in that channel"));
                return;
            }

            try
            {
                await channel.SendMessageAsync(message, embed: EmbedHelper.CreateSmallEmbed(Context.User).Build(),
                    allowedMentions: AllowedMentions.None);
                await Context.Message.DeleteAsync();
            }
            catch (Exception e)
            {
                await ReplyAsync(embed: EmbedHelper.ErrorEmbed($"Couldn't send message to channel: `{e.Message}`"));
            }

        }
    }
}