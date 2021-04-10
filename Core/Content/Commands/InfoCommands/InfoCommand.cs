using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TomatBot.Core.Content.Embeds;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;

namespace TomatBot.Core.Content.Commands.InfoCommands
{
    public sealed class InfoCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData => new("info", "Displays standard information about the bot, including developer contact information and a bot invitation link.");

        public override CommandType CType => CommandType.Info;

        [Command("info")]
        [Summary("Sends an embed with basic information about the bot.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public Task HandleCommand()
        {
            BaseEmbed embed = new(Context.User)
            {
                Title = "Basic Bot Info",

                Description = "`Tomat` is a general-use bot by `convicted tomatophile#0001`." +
                              "\n<pending> (features)" +
                              "\n\nFor command help, use `tomat!help`" +
                              "\nIf you have any other questions, give Stevie a DM." +
                              "\nInvite: ||<https://discord.com/api/oauth2/authorize?client_id=800480899300065321&permissions=8&scope=bot>||"
            };

            return ReplyAsync(embed: embed.Build());
        }
    }
}
