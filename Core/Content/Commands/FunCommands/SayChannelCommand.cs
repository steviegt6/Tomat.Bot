using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;

namespace TomatBot.Core.Content.Commands.FunCommands
{
    public sealed class SayChannelCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData =>
            new HelpCommandData("saychannel",
                "Echoes the text specified in the given channel, deletes the message from the user afterward.");

        public override CommandType CType => CommandType.Fun;

        [Command("saychannel")]
        [Alias("saych", "chsay", "channelsay")]
        [Summary("Echoes back a message in the specified channel.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public Task HandleCommand(
            [Summary("The channel you want the text to be displayed in")]
            SocketChannel channel,

            [Remainder]
            [Summary("The text you wish you echo.")]
            string message)
        {
            SocketGuild? neededGuild = BotStartup.Client?.Guilds.FirstOrDefault(x => x == Context.Guild) ?? null;

            Context.Message.DeleteAsync();

            if (!(channel is IMessageChannel messageChannel) || !(channel is IGuildChannel guildChannel))
                return ReplyAsync(embed: CreateSmallEmbed("Specified channel not found!").Build());

            if (neededGuild == null)
            {
                return messageChannel.SendMessageAsync(message,
                    embed: CreateSmallEmbed("Guild somehow not found!").Build());
            }

            if (!neededGuild.CurrentUser.GetPermissions(guildChannel).ToList().Contains(ChannelPermission.SendMessages))
                return ReplyAsync(
                    embed: CreateSmallEmbed(
                        "Bot is lacking sufficient permissions to send a message to the specified channel.").Build());

            if (Context.User is SocketGuildUser guildUser && !guildUser.GetPermissions(guildChannel).ToList()
                .Contains(ChannelPermission.SendMessages))
                return ReplyAsync(
                    embed: CreateSmallEmbed(
                        "User is lacking sufficient permissions to send a message to the specified channel.").Build());

            return messageChannel.SendMessageAsync(message,
                embed: CreateSmallEmbed().Build());
        }
    }
}
