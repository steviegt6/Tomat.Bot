using System;
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
    public sealed class RandomNumberCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData => new("random", "Picks a random number between zero and the specified number, or the first number and the second number.");

        public override CommandType CType => CommandType.Fun;

        public override string Parameters => "<numberone> [numbertwo]";

        [Command("random")]
        [Alias("rand", "randomnumber", "randnum", "randomnum")]
        [Summary("Picks a random number.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task HandleCommand(
            string firstNumber,
            [Remainder] string? secondNumber)
        {
            try
            {
                Random numberGenerator = new();
                string response = string.IsNullOrEmpty(secondNumber)
                    ? numberGenerator.Next(int.Parse(firstNumber) + 1).ToString()
                    : numberGenerator.Next(int.Parse(firstNumber), int.Parse(secondNumber) + 1).ToString();

                await ReplyAsync(response, embed: CreateSmallEmbed().Build());
            }
            catch (Exception e)
            {
                BaseEmbed embed = CreateSmallEmbed(e.Message);
                embed.WithTitle(e.GetType().Name);
                embed.WithColor(Color.Red);
                await ReplyAsync(embed: embed.Build());
            }
        }
    }
}
