using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Tomat.CommandFramework;
using Tomat.CommandFramework.HelpSystem;
using Tomat.Conveniency.Embeds;
using Tomat.Conveniency.Utilities;

namespace TomatBot.Content.Commands.InfoCommands
{
    public sealed class InfoCommand : BaseCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData =>
            new("info",
                "Displays standard information about the bot, including developer contact information and a bot invitation link.");

        public override CommandType CType => CommandType.Info;

        [Command("info")]
        [Summary("Sends an embed with basic information about the bot.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public Task HandleCommand()
        {
            BaseEmbed embed = new(Context.User)
            {
                Title = "Basic Bot Info",

                Description =
                    "`Tomat` is a general-purpose Discord bot programmed by `convicted tomatophile#0001` with the help of `TheStachelfisch#0395`, who also hosts." +
                    $"\n\nFor command help, use `{BotStartup.DefaultPrefix}help` or ping the bot directly (`@Tomat help`)" +
                    $"\nBot up-time: `{BotStartup.UpTime.FormatToString(true)}`" +
                    "\n\n\nClick [here](https://discord.com/api/oauth2/authorize?client_id=800480899300065321&permissions=93248&scope=bot) to invite Tomat to your server."
            };

            return ReplyAsync(embed: embed.Build());
        }
    }
}