using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TomatBot.API.Web.EitherIO;
using TomatBot.Core.Content.Embeds;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;
using TomatBot.Core.Utilities;

namespace TomatBot.Core.Content.Commands.FunCommands
{
    public sealed class EitherCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData => new("either", "Displays a Would-You-Rather question taken directly from [either.io](http://either.io/).");

        public override CommandType CType => CommandType.Fun;

        [Command("either")]
        [Summary("Displays two fun options for you to select (taken from [either.io](http://either.io/)).")]
        [RequireBotPermission(ChannelPermission.SendMessages)]

        public async Task HandleCommand()
        {
            EitherIORequest? options = EitherIORequest.MakeRequest();

            if (!options.HasValue)
            {
                Embed embed = EmbedHelper.ErrorEmbed(
                    "Error while attempting to make a request to [either.io](http://either.io/). If the website is up and this issue persists, contact a developer.",
                    new BaseEmbed(Context.User).Footer);
                await ReplyAsync(embed: embed);
                return;
            }

            BaseEmbed realEmbed = new(Context.User)
            {
                Title = "Would you rather...",

                Description = $":one: {options.Value.optionOne}" +
                              $"\n:two: {options.Value.optionTwo}"
            };

            await ReplyAsync(embed: realEmbed.Build());
        }
    }
}
