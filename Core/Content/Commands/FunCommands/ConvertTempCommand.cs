using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TomatBot.Core.Content.Embeds;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;
using TomatBot.Core.Utilities;

namespace TomatBot.Core.Content.Commands.FunCommands
{
    public sealed class ConvertTempCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData => new("convert", "Converts specified number from celsius to farenheit, or vice versa.");

        public override CommandType CType => CommandType.Fun;

        public override string Parameters => "<temperature>";

        [Command("temp")]
        [Alias("temperature", "convertTemp", "tempConvert")]
        [Summary("Converts temperatures!")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task HandleCommand(string temp)
        {
            // Only get numbers from string and then convert to Fahrenheit
            ;
            double numbers = double.Parse(Regex.Match(temp, @"^-?[0-9]\d*(\.\d+)?").Value);
            
            // Check if Celsius was mentioned
            if (temp.Contains("C", StringComparison.OrdinalIgnoreCase) || temp.Contains("Celsius", StringComparison.OrdinalIgnoreCase))
            {
                double convertedFahrenheit = Math.Round((double)numbers * 9 / 5 + 32, 2);

                await ReplyAsync(embed: new EmbedBuilder
                {
                    Title = "Converted Fahrenheit",
                    Description = $"{numbers}°C = {convertedFahrenheit}°F",
                    Color = Color.DarkBlue
                }.Build());
            }
            else if (temp.Contains("F", StringComparison.OrdinalIgnoreCase) || temp.Contains("Fahrenheit", StringComparison.OrdinalIgnoreCase))
            {
                // Avoid loss of fraction
                double convertedCelsius = Math.Round((numbers - 32.0) * 5 / 9, 2);
                
                await ReplyAsync(embed: new EmbedBuilder
                {
                    Title = "Converted Celsius",
                    Description = $"{numbers}°F = {convertedCelsius}°C",
                    Color = Color.DarkBlue
                }.Build());
            }
            // If its any other unit than Celsius or Fahrenheit
            else
            {
                await ReplyAsync(embed: EmbedHelper.ErrorEmbed("Unknown unit. Currently only supports Fahrenheit and Celsius"));
            }
        }
    }
}