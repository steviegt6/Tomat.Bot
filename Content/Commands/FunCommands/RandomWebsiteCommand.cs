using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Tomat.CommandFramework;
using Tomat.CommandFramework.HelpSystem;
using Tomat.Conveniency.Embeds;

namespace TomatBot.Content.Commands.FunCommands
{
    public sealed class RandomWebsiteCommand : BaseCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData =>
            new("randomwebsite",
                "Links the user to a random website from a list of websites provided by [TheUselessWeb](https://theuselessweb.com/).");

        public override CommandType CType => CommandType.Fun;

        [Command("randomwebsite")]
        [Alias("randweb", "ranweb", "rw")]
        [Summary("Gives a link to a random, useless website.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task HandleCommand()
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
    }
}