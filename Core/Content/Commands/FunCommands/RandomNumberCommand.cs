using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TomatBot.Core.Content.Embeds;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;

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
            string secondNumber = "")
        {
            try
            {
                Random numberGenerator = new();
                string response = string.IsNullOrEmpty(secondNumber)
                    ? numberGenerator.Next(int.Parse(firstNumber) + 1).ToString()
                    : numberGenerator.Next(int.Parse(firstNumber), int.Parse(secondNumber) + 1).ToString();

                if (int.Parse(firstNumber) == 0)
                    throw new InvalidOperationException("The first number should be greater than zero, or it will always return zero!");

                if (!string.IsNullOrEmpty(secondNumber) && int.Parse(firstNumber) >= int.Parse(secondNumber))
                    throw new InvalidOperationException(
                        "The second number should be *greater* than the first number, not less than or equal to the first number!");

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
