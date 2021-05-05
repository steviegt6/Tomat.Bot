using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Tomat.CommandFramework;
using Tomat.CommandFramework.HelpSystem;
using Tomat.Conveniency.Embeds;
using Tomat.Conveniency.Utilities;
using Tomat.MiscWeb.EitherIO;

namespace TomatBot.Content.Commands.FunCommands
{
    public sealed class EitherCommand : BaseCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData =>
            new("either", "Displays a Would-You-Rather question taken directly from [either.io](http://either.io/).");

        public override CommandType CType => CommandType.Fun;

        [Command("either")]
        [Alias("wyr", "wouldyourather")]
        [Summary("Displays two fun options for you to select (taken from [either.io](http://either.io/)).")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task HandleCommand()
        {
            EitherIORequest options = EitherIORequest.MakeRequest();

            if (!string.IsNullOrEmpty(options.exception))
            {
                Embed embed = EmbedHelper.ErrorEmbed(
                    "Error while attempting to make a request to [either.io](http://either.io/). If the website is up and this issue persists, contact a developer." +
                    $"\n\nException: {options.exception}",
                    new BaseEmbed(Context.User).Footer);
                await ReplyAsync(embed: embed);
                return;
            }

            BaseEmbed realEmbed = new(Context.User)
            {
                Title = "Would you rather...",

                Description = $":one: {options.optionOne}" +
                              $"\n:two: {options.optionTwo}"
            };

            IUserMessage? message = await ReplyAsync(embed: realEmbed.Build());
            await message.AddReactionAsync(new Emoji("1️⃣"));
            await message.AddReactionAsync(new Emoji("2️⃣"));
        }
    }
}