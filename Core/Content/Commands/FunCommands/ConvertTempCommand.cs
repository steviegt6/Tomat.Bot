using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TomatBot.Core.Content.Embeds;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;

namespace TomatBot.Core.Content.Commands.FunCommands
{
    public sealed class ConvertTempCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData => new("convert", "Converts specified number from celsius to farenheit, or vice versa.");

        public override CommandType CType => CommandType.Fun;

        public override string Parameters => "<temperature>";

        [Command("converttemp")]
        [Alias("convtemp","tempconv", "tempconvert")]
        [Summary("Converts temperatures!")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task HandleCommand(
            string temp = "")
        {
            try
            {
                int tempNumber;
                double inCelsius, inFahrenheit;
                string response;
                //if the number cannot be read, throw error.
                if(!int.TryParse(temp, out tempNumber))
                {
                    throw new InvalidOperationException("Could not read the temperature value!");
                }
                //convert user input into both units of temperature
                inFahrenheit = (tempNumber * 9) / 5 + 32;
                inCelsius = ((tempNumber - 32) * 5) / 9;
                //construct response.
                response = temp + " celsius is equal to " + inFahrenheit + " fahrenheit!\n" +
                temp + " fahrenheit is equal to " + inCelsius + " celsius!"; 

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
