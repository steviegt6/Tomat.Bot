using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;
using TomatBot.Core.Utilities;

namespace TomatBot.Core.Content.Commands.FunCommands
{
    public sealed class SayChannelCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData =>
            new("saychannel",
                "Echoes the text specified in the given channel, deletes the message from the user afterward.");

        public override CommandType CType => CommandType.Fun;

        public override string? Parameters => "<channel> <message>";

        [Command("saychannel")]
        [Alias("saych", "chsay", "channelsay")]
        [Summary("Echoes back a message in the specified channel.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task HandleCommand(
            [Summary("The channel you want the text to be displayed in")]
            SocketTextChannel channel,

            [Remainder]
            [Summary("The text you wish you echo.")]
            string message)
        {
            if ((Context.User as SocketGuildUser)!.GetPermissions(Context.Channel as IGuildChannel).SendMessages!)
            {
                await ReplyAsync(embed:EmbedHelper.ErrorEmbed("You don't have permissions to send messages in that channel"));
                return;
            }

            try
            {
                await channel.SendMessageAsync(message, embed: CreateSmallEmbed().Build(), allowedMentions:AllowedMentions.None);
                await Context.Message.DeleteAsync();
            }
            catch (Exception e)
            {
                await ReplyAsync($"Couldn't send message to channel: `{e.Message}`");
            }
            
        }
    }
}
