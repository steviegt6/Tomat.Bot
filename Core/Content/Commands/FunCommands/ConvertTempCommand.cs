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

        public override string Parameters => "<celsius/farenheit> <temperature>";

        [Command("convert")]
        [Alias("conv")]
        [Summary("Converts temperatures!")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task HandleCommand(
            string unit,
            string temp = "")
        {
            try
            {
                int tempNumber;
                double convertedTemp;
                string response;
                if(!int.TryParse(temp, out tempNumber))
                {
                    throw new InvalidOperationException("Could not read the temperature value!");
                }
                //celsius to fahrenheit
                if(unit == "c" || unit == "celsius")
                {
                    convertedTemp = (tempNumber * 9) / 5 + 32;
                    response = temp + " celsius is equal to " + convertedTemp + " fahrenheit!"; 
                }
                //fahrenheit to celsius
                if (unit == "f" || unit == "fahrenheit")
                {
                    convertedTemp = ((tempNumber - 32) * 5) / 9;
                    response = temp + " fahrenheit is equal to " + convertedTemp + " celsius!";
                }
                //throw exception if the checks fail
                else
                {
                    throw new InvalidOperationException("Could not read the temperature value or unit, try writing celsius or c!");
                }

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
