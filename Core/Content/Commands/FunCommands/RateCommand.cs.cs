using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using TomatBot.Core.Content.Configs;
using TomatBot.Core.Content.Embeds;
using TomatBot.Core.Content.Services;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;

namespace TomatBot.Core.Content.Commands.FunCommands
{
    public sealed class RateCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData => new("rate", "Rates whatever you want. Chooses a number between 0 and 10, not biased at all and puts lots of thought into ratings!");

        public override CommandType CType => CommandType.Fun;

        public override string Parameters => "<object-to-rate>";

        [Command("rate")]
        [Alias("ratewaifu")] // command name familiarity
        [Summary("Rates whatever you want it to.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task HandleCommand(
            [Remainder]
            string objectToRate)
        {
            GlobalConfig config = BotStartup.Provider.GetRequiredService<ConfigService>().Config.Global;
            int rating;
            int requestCount;

            if (config.ratings.ContainsKey(objectToRate.ToLower()))
            {
                (int savedRating, int totalCount) = config.ratings[objectToRate.ToLower()];
                rating = savedRating;
                requestCount = ++totalCount;

                config.ratings[objectToRate.ToLower()] = (savedRating, totalCount);
            }
            else
            {
                rating = new Random().Next(0, 11);
                requestCount = 1;
                config.ratings.Add(objectToRate.ToLower(), (rating, requestCount));
            }

            BaseEmbed embed = new(Context.User)
            {
                Title = $"I give \"{objectToRate}\"...",
                Description = $"...a {rating}/10!" +
                              $"\n\nI have been asked for {objectToRate}'{(objectToRate.Last().Equals('s') ? "" : "s")} rating {requestCount} time{(requestCount == 1 ? "" : "s")}."
            };

            await ReplyAsync(embed: embed.Build());
        }
    }
}
