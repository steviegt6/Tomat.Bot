using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TomatBot.Core.Content.Embeds;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;

namespace TomatBot.Core.Content.Commands.InfoCommands
{
    public sealed class PermissionsCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData => new HelpCommandData("permissions", "Lists all current guild and channel permissions the bot has.");

        public override CommandType CType => CommandType.Info;

        [Command("permissions")]
        [Alias("perms")]
        [Summary("Lists the bot's current permissions.")]
        public Task HandleCommand()
        {
            string channelPerms = "";
            string guildPerms;
            SocketGuild? neededGuild = BotStartup.Client?.Guilds.FirstOrDefault(x => x == Context.Guild) ?? null;

            if (neededGuild != null)
            {
                guildPerms = neededGuild.CurrentUser.GuildPermissions.ToList()
                    .Aggregate("", (current, gPerm) => current + $"\n * {gPerm}");

                if (Context.Channel is IGuildChannel guildChannel)
                    channelPerms = neededGuild.CurrentUser.GetPermissions(guildChannel).ToList()
                        .Aggregate("", (current, cPerm) => current + $"\n * {cPerm}");
                else
                    channelPerms = "unable to fetch current channel";
            }
            else
                guildPerms = "unable to fetch guild";

            BaseEmbed embed = new BaseEmbed(Context.User)
            {
                Title = "Permissions",

                Fields = new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        IsInline = true,
                        Name = "Guild Permissions",
                        Value = guildPerms
                    },

                    new EmbedFieldBuilder
                    {
                        IsInline = true,
                        Name = "Channel Permissions",
                        Value = channelPerms
                    }
                }
            };

            return ReplyAsync(embed: embed.Build());
        }
    }
}
