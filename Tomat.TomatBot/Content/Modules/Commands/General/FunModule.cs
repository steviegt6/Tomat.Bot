#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Tomat.TomatBot.Common.Embeds;
using Tomat.TomatBot.Common.Web.EitherIO;
using Tomat.TomatBot.Content.Configuration;
using Tomat.TomatBot.Content.Modules.Commands.General.Temperature;
using Tomat.TomatBot.Core.CommandContext;
using Tomat.TomatBot.Core.Services.Commands;

namespace Tomat.TomatBot.Content.Modules.Commands.General
{
    [ModuleInfo("Fun & Misc.")]
    public sealed class FunModule : ModuleBase<BotCommandContext>
    {
        #region Temperature Conversion

        public Dictionary<TempUnit, ITempConverter> TemperatureConverters = new()
        {
            { TempUnit.Fahrenheit, new FahrenheitConverter() },
            { TempUnit.Celsius, new CelsiusConverter() },
            { TempUnit.Kelvin, new KelvinConverter() },
            { TempUnit.Reaumur, new ReaumurConverter() },
            { TempUnit.Rankine, new RankineConverter() }
        };

        [Command("temp")]
        [Alias("temperature", "converttemp", "tempconvert", "convert")]
        [Summary("Converts temperatures to: fahrenheit, celsius, kelvin, rankine, and réaumur!")]
        [Parameters("<temperature><symbol suffix>")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task ConvertTepAsync(string temp)
        {
            // Only get numbers from string.
            double numbers = double.Parse(Regex.Match(temp, @"^-?[0-9]\d*(\.\d+)?").Value);
            TempUnit? unitType = null;

            foreach (ITempConverter converter in TemperatureConverters.Values.Where(x =>
                temp.EndsWith(x.ShortName, StringComparison.OrdinalIgnoreCase) ||
                temp.EndsWith(x.LongName, StringComparison.OrdinalIgnoreCase)))
            {
                unitType = converter.AssociatedUnit;
                break;
            }

            if (!unitType.HasValue)
            {
                string units = "";

                foreach ((TempUnit unit, var converter) in TemperatureConverters)
                    units += $"\n{unit}: {converter.LongName}/{converter.ShortName}";

                await ReplyAsync(embed: EmbedHelper.ErrorEmbed("Invalid temperature type. Please specify a unit:" +
                                                               $"{units}"));
                return;
            }

            ITempConverter tConverter = TemperatureConverters[unitType.Value];

            string title = $"Converted {tConverter.LongName}";

            await ReplyAsync(embed: new BaseEmbed(Context.Bot, Context.User, Color.DarkBlue)
            {
                Title = $"Converted {tConverter.LongName}",
                Description = GetTempText(numbers, tConverter)
            }.Build());
        }

        public string GetTempText(double temp, ITempConverter converter)
        {
            StringBuilder builder = new();

            void AddUnit(TempUnit newUnit)
            {
                string left = temp + converter.TextRepresentation;
                ITempConverter newConv = TemperatureConverters[newUnit];
                double rounded = Math.Round(newConv.ToTemp(temp, converter.AssociatedUnit));

                builder.AppendLine($"{left} = {rounded}{newConv.TextRepresentation}");
            }

            foreach (TempUnit tUnit in TemperatureConverters.Keys)
                AddUnit(tUnit);


            return builder.ToString();
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

            if (!string.IsNullOrEmpty(options.Exception))
            {
                Embed embed = EmbedHelper.ErrorEmbed(
                    "Error while attempting to make a request to [either.io](http://either.io/)." +
                    "\nIf the website is up and this issue persists, contact a developer." +
                    $"\n\nException: {options.Exception}",
                    new BaseEmbed(Context.Bot, Context.User).Footer);
                await ReplyAsync(embed: embed);
                return;
            }

            BaseEmbed realEmbed = new(Context.Bot, Context.User)
            {
                Title = "Would you rather...",

                Description = $":one: {options.Options.OptionOne}" +
                              $"\n:two: {options.Options.OptionTwo}"
            };

            IUserMessage? message = await ReplyAsync(embed: realEmbed.Build());
            await message.AddReactionAsync(new Emoji("1️⃣"));
            await message.AddReactionAsync(new Emoji("2️⃣"));
        }

        #endregion

        #region Randomization

        public static readonly string[] Websites =
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

        [Command("randomwebsite")]
        [Alias("randweb", "ranweb", "rw")]
        [Summary("Links to a random website from a list provided by [TheUselessWeb](https://theuselessweb.com/).")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task RandomWebsiteAsync()
        {
            BaseEmbed embed = new(Context.Bot, Context.User)
            {
                Title = "Random Useless Website",

                Description =
                    $"Click [here]({Websites[new Random().Next(Websites.Length)]}) to go to a random, useless website." +
                    "\nWant another one? Run this command again." +
                    "\n\nWebsites taken from [TheUselessWeb](https://theuselessweb.com/)."
            };

            await ReplyAsync(embed: embed.Build());
        }

        #endregion

        #region Rate Command

        [Command("rate")]
        [Alias("ratewaifu")] // command name familiarity ;)
        [Summary("Rates whatever you want." +
                 "\nChooses a number between 0 and 10, not biased at all and puts lots of thought into ratings!")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [Parameters("<object>")]
        public async Task RateAsync([Remainder] string objectToRate)
        {
            if (Context.Bot is not TomatBot tomat)
                return;

            GlobalData config = tomat.GlobalConfig.Data;
            int rating;
            int requestCount;

            if (config.Ratings.ContainsKey(objectToRate.ToLower()))
            {
                (int savedRating, int totalCount) = config.Ratings[objectToRate.ToLower()];
                rating = savedRating;
                requestCount = ++totalCount;

                config.Ratings[objectToRate.ToLower()] = (savedRating, totalCount);
            }
            else
            {
                rating = new Random().Next(0, 11);
                requestCount = 1;
                config.Ratings.Add(objectToRate.ToLower(), (rating, requestCount));
            }

            _ = MentionUtils.TryParseUser(objectToRate, out ulong user);

            string title = user == 0 ? objectToRate : Context.Client.Rest.GetUserAsync(user).Result.Username;
            string es1 = objectToRate.Last().Equals('s') ? "" : "s";
            string es2 = requestCount == 1 ? "" : "s";

            BaseEmbed embed = new(tomat, Context.User)
            {
                Title = $"I give \"{title}\"...",
                Description = $"...a {rating}/10!" +
                              $"\n\nI have been asked for {objectToRate}'{es1} rating {requestCount} time{es2}."
            };

            await ReplyAsync(embed: embed.Build());
        }

        #endregion
    }
}