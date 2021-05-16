using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Tomat.Conveniency.Embeds;
using Tomat.Conveniency.Utilities;
using Tomat.MiscWeb.EitherIO;
using Tomat.TomatBot.Common;
using Tomat.TomatBot.Content.Configs;
using Tomat.TomatBot.Content.Services;

namespace Tomat.TomatBot.Content.Commands
{
    public sealed class FunModule : ModuleBase<SocketCommandContext>
    {
        #region Temperature Conversion

        public enum TempUnit
        {
            Fahrenheit,
            Celsius,
            Kelvin,
            Rankine,
            Reaumur
        }

        [Command("temp")]
        [Alias("temperature", "converttemp", "tempconvert", "convert")]
        [Summary("Converts temperatures to: fahrenheit, celsius, kelvin, rankine, and réaumur!")]
        [Parameters("<temperature> (no symbol suffix)")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task ConvertTempAsync(string temp)
        {
            // Only get numbers from string and then convert to Fahrenheit
            double numbers = double.Parse(Regex.Match(temp, @"^-?[0-9]\d*(\.\d+)?").Value);

            // Check if Celsius was mentioned
            if (temp.EndsWith("F", StringComparison.OrdinalIgnoreCase) ||
                temp.EndsWith("Fahrenheit", StringComparison.OrdinalIgnoreCase))
            {
                await ReplyAsync(embed: new EmbedBuilder
                {
                    Title = "Converted Fahrenheit",
                    Description = GetTempText(numbers, "°F", TempUnit.Fahrenheit),
                    Color = Color.DarkBlue
                }.Build());
            }
            else if (temp.EndsWith("C", StringComparison.OrdinalIgnoreCase) ||
                     temp.EndsWith("Celsius", StringComparison.OrdinalIgnoreCase))
            {
                await ReplyAsync(embed: new EmbedBuilder
                {
                    Title = "Converted Celsius",
                    Description = GetTempText(numbers, "°C", TempUnit.Celsius),
                    Color = Color.DarkBlue
                }.Build());
            }
            else if (temp.EndsWith("K", StringComparison.OrdinalIgnoreCase) ||
                     temp.EndsWith("Kelvin", StringComparison.OrdinalIgnoreCase))
            {
                await ReplyAsync(embed: new EmbedBuilder
                {
                    Title = "Converted Kelvin",
                    Description = GetTempText(numbers, "K", TempUnit.Kelvin),
                    Color = Color.DarkBlue
                }.Build());
            }
            else if (temp.EndsWith("Ra", StringComparison.OrdinalIgnoreCase) ||
                     temp.EndsWith("Rankine", StringComparison.OrdinalIgnoreCase))
            {
                await ReplyAsync(embed: new EmbedBuilder
                {
                    Title = "Converted Rankine",
                    Description = GetTempText(numbers, "°Ra", TempUnit.Rankine),
                    Color = Color.DarkBlue
                }.Build());
            }
            else if (temp.EndsWith("Re", StringComparison.OrdinalIgnoreCase) ||
                     temp.EndsWith("Reaumur", StringComparison.OrdinalIgnoreCase))
            {
                await ReplyAsync(embed: new EmbedBuilder
                {
                    Title = "Converted Réaumur",
                    Description = GetTempText(numbers, "°Re", TempUnit.Reaumur),
                    Color = Color.DarkBlue
                }.Build());
            }
            // If its any other unit than Celsius or Fahrenheit
            else
                await ReplyAsync(
                    embed: EmbedHelper.ErrorEmbed(
                        "Please specify a unit (F, C, K, Ra, Re, Fahrenheit, Celsius, Kelvin, Rankine, Reaumur)."));
        }

        private static string GetTempText(double temp, string unitCh, TempUnit unit) =>
            $"{temp}{unitCh} = {Math.Round(ToFahrenheit(temp, unit), 2)}°F" +
            $"\n{temp}{unitCh} = {Math.Round(ToCelsius(temp, unit), 2)}°C" +
            $"\n{temp}{unitCh} = {Math.Round(ToKelvin(temp, unit), 2)}K," +
            $"\n{temp}{unitCh} = {Math.Round(ToRankine(temp, unit), 2)}°Ra" +
            $"\n{temp}{unitCh} = {Math.Round(ToReaumur(temp, unit), 2)}°Re";

        public static double ToFahrenheit(double temp, TempUnit fromUnit)
        {
            return fromUnit switch
            {
                TempUnit.Fahrenheit => temp,
                TempUnit.Celsius => temp * 1.8 + 32,
                TempUnit.Kelvin => temp * 1.8 - 459.67,
                TempUnit.Rankine => temp - 459.67,
                TempUnit.Reaumur => temp * 2.25 + 32,
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }

        public static double ToCelsius(double temp, TempUnit fromUnit)
        {
            return fromUnit switch
            {
                TempUnit.Fahrenheit => (temp - 32) / 1.8,
                TempUnit.Celsius => temp,
                TempUnit.Kelvin => temp - 273.15,
                TempUnit.Rankine => (temp - 32 - 459.67) / 1.8,
                TempUnit.Reaumur => temp * 1.25,
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }

        public static double ToKelvin(double temp, TempUnit fromUnit)
        {
            return fromUnit switch
            {
                TempUnit.Fahrenheit => (temp + 459.67) / 1.8,
                TempUnit.Celsius => temp + 273.15,
                TempUnit.Kelvin => temp,
                TempUnit.Rankine => temp / 1.8,
                TempUnit.Reaumur => temp * 1.25 + 273.15,
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }

        public static double ToRankine(double temp, TempUnit fromUnit)
        {
            return fromUnit switch
            {
                TempUnit.Fahrenheit => temp + 459.67,
                TempUnit.Celsius => temp * 1.8 + 32 + 459.67,
                TempUnit.Kelvin => temp * 1.8,
                TempUnit.Rankine => temp,
                TempUnit.Reaumur => temp * 2.25 + 32 + 459.67,
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }

        public static double ToReaumur(double temp, TempUnit fromUnit)
        {
            return fromUnit switch
            {
                TempUnit.Fahrenheit => (temp - 32) / 2.25,
                TempUnit.Celsius => temp * 0.8,
                TempUnit.Kelvin => (temp - 273.15) * 0.8,
                TempUnit.Rankine => (temp - 32 - 459.67) / 2.25,
                TempUnit.Reaumur => temp,
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }

        #endregion

        #region Either Command

        [Command("either")]
        [Alias("wyr", "wouldyourather")]
        [Summary("Displays a Would-You-Rather question taken directly from [either.io](http://either.io/).")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task EitherAsync()
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

        #endregion

        #region Randomization

        [Command("random")]
        [Alias("rand", "randomnumber", "randnum", "randomnum")]
        [Summary(
            "Picks a random number between zero and the specified number, or the first number and the second number.")]
        [Parameters("<max> OR <min> <max>")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task RandomNumberAsync(int firstNumber, int secondNumber = int.MinValue)
        {
            try
            {
                Random numberGenerator = new();
                string response = secondNumber == int.MinValue // compensate for inclusive-exclusivity with adding 1
                    ? numberGenerator.Next(firstNumber + 1).ToString()
                    : numberGenerator.Next(firstNumber, secondNumber + 1).ToString();

                if (firstNumber == 0)
                    throw new InvalidOperationException(
                        "The first number should be greater than zero, or it will always return zero!");

                if (secondNumber != int.MinValue && firstNumber >= secondNumber)
                    throw new InvalidOperationException(
                        "The second number should be *greater* than the first number, not less than or equal to the first number!");

                await ReplyAsync(response, embed: EmbedHelper.CreateSmallEmbed(Context.User).Build());
            }
            catch (Exception e)
            {
                BaseEmbed embed = EmbedHelper.CreateSmallEmbed(Context.User, e.Message);
                embed.WithTitle(e.GetType().Name);
                embed.WithColor(Color.Red);
                await ReplyAsync(embed: embed.Build());
            }
        }

        [Command("randomwebsite")]
        [Alias("randweb", "ranweb", "rw")]
        [Summary(
            "Links the user to a random website from a list of websites provided by [TheUselessWeb](https://theuselessweb.com/).")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task RandomWebsiteAsync()
        {
            BaseEmbed embed = new(Context.User)
            {
                Title = "Random Useless Website",

                Description =
                    $"Click [here]({Websites[new Random().Next(Websites.Length)]}) to go to a random, useless website. Want another one? Run this command again." +
                    "\n\nWebsites taken from [TheUselessWeb](https://theuselessweb.com/)."
            };

            await ReplyAsync(embed: embed.Build());
        }

        private static readonly string[] Websites =
        {
            "https://longdogechallenge.com/",
            "http://heeeeeeeey.com/",
            "http://corndog.io/",
            "https://mondrianandme.com/",
            "https://puginarug.com",
            "https://alwaysjudgeabookbyitscover.com",
            "https://thatsthefinger.com/",
            "https://cant-not-tweet-this.com/",
            "https://weirdorconfusing.com/",
            "http://eelslap.com/",
            "http://www.staggeringbeauty.com/",
            "http://burymewithmymoney.com/",
            "https://smashthewalls.com/",
            "https://jacksonpollock.org/",
            "http://endless.horse/",
            "https://www.trypap.com/",
            "http://www.republiquedesmangues.fr/",
            "http://www.movenowthinklater.com/",
            "http://www.partridgegetslucky.com/",
            "http://www.rrrgggbbb.com/",
            "http://beesbeesbees.com/",
            "http://www.koalastothemax.com/",
            "http://www.everydayim.com/",
            "http://randomcolour.com/",
            "http://cat-bounce.com/",
            "http://chrismckenzie.com/",
            "https://thezen.zone/",
            "http://hasthelargehadroncolliderdestroyedtheworldyet.com/",
            "http://ninjaflex.com/",
            "http://ihasabucket.com/",
            "http://corndogoncorndog.com/",
            "http://www.hackertyper.com/",
            "https://pointerpointer.com",
            "http://imaninja.com/",
            "http://drawing.garden/",
            "http://www.ismycomputeron.com/",
            "http://www.nullingthevoid.com/",
            "http://www.muchbetterthanthis.com/",
            "http://www.yesnoif.com/",
            "http://lacquerlacquer.com",
            "http://potatoortomato.com/",
            "http://iamawesome.com/",
            "https://strobe.cool/",
            "http://www.pleaselike.com/",
            "http://crouton.net/",
            "http://corgiorgy.com/",
            "http://www.wutdafuk.com/",
            "http://unicodesnowmanforyou.com/",
            "http://chillestmonkey.com/",
            "http://scroll-o-meter.club/",
            "http://www.crossdivisions.com/",
            "http://tencents.info/",
            "https://boringboringboring.com/",
            "http://www.patience-is-a-virtue.org/",
            "http://pixelsfighting.com/",
            "http://isitwhite.com/",
            "https://existentialcrisis.com/",
            "http://onemillionlols.com/",
            "http://www.omfgdogs.com/",
            "http://oct82.com/",
            "http://chihuahuaspin.com/",
            "http://www.blankwindows.com/",
            "http://dogs.are.the.most.moe/",
            "http://tunnelsnakes.com/",
            "http://www.trashloop.com/",
            "http://www.ascii-middle-finger.com/",
            "http://spaceis.cool/",
            "http://www.donothingfor2minutes.com/",
            "http://buildshruggie.com/",
            "http://buzzybuzz.biz/",
            "http://yeahlemons.com/",
            "http://wowenwilsonquiz.com",
            "https://thepigeon.org/",
            "http://notdayoftheweek.com/",
            "http://www.amialright.com/",
            "http://nooooooooooooooo.com/",
            "https://greatbignothing.com/",
            "https://zoomquilt.org/",
            "https://dadlaughbutton.com/",
            "https://www.bouncingdvdlogo.com/",
            "https://remoji.com/",
            "http://papertoilet.com/"
        };

        #endregion

        #region Rate Command

        [Command("rate")]
        [Alias("ratewaifu")] // command name familiarity
        [Summary(
            "Rates whatever you want. Chooses a number between 0 and 10, not biased at all and puts lots of thought into ratings!")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [Parameters("<object>")]
        public async Task RateAsync([Remainder] string objectToRate)
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

            _ = MentionUtils.TryParseUser(objectToRate, out ulong user);

            BaseEmbed embed = new(Context.User)
            {
                Title =
                    $"I give \"{(user == 0 ? objectToRate : Context.Client.Rest.GetUserAsync(user).Result.Username)}\"...",
                Description = $"...a {rating}/10!" +
                              $"\n\nI have been asked for {objectToRate}'{(objectToRate.Last().Equals('s') ? "" : "s")} rating {requestCount} time{(requestCount == 1 ? "" : "s")}."
            };

            await ReplyAsync(embed: embed.Build());
        }

        #endregion

        #region Say Stuff

        [Command("say")]
        [Summary("Echoes back a message.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [Parameters("<message>")]
        public async Task SayAsync([Remainder] string message)
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(message, embed: EmbedHelper.CreateSmallEmbed(Context.User).Build(),
                allowedMentions: AllowedMentions.None);
        }

        [Command("saychannel")]
        [Alias("saych", "chsay", "channelsay")]
        [Summary("Echoes back a message in the specified channel.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [Parameters("<channel> <message>")]
        public async Task SayChannelAsync(
            SocketTextChannel channel, [Remainder] string message)
        {
            if (!(Context.User as SocketGuildUser)!.GetPermissions(channel).SendMessages)
            {
                await ReplyAsync(
                    embed: EmbedHelper.ErrorEmbed("You don't have the permission to send messages in that channel"));
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

        #endregion

        #region Suggest Command

        [Command("suggestion")]
        [Alias("suggest")]
        [Summary("Creates a suggestion embed with a +1 and -1 option.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [RequireBotPermission(ChannelPermission.AddReactions)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task SuggestAsync([Remainder] string suggestion)
        {
            await Context.Message.DeleteAsync();

            EmbedBuilder realEmbed = new BaseEmbed(Context.User)
            {
                Title = "Suggestion!",
                Color = Color.Gold,
                Description = suggestion
            };

            IUserMessage? message = await ReplyAsync(embed: realEmbed.Build());
            await message.AddReactionsAsync(new IEmote[] { new Emoji("👍"), new Emoji("👎") });
        }

        #endregion
    }
}