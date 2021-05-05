using Discord;
using Discord.Commands;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;
using TomatBot.Core.Utilities;

namespace TomatBot.Core.Content.Commands.FunCommands
{
    public sealed class ConvertTempCommand : TomatCommand
    {
        public enum Unit
        {
            Fahrenheit,
            Celsius,
            Kelvin,
            Rankine,
            Reaumur
        }

        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData => new("temp", "Converts specified number from celsius to fahrenheit, or vice versa.");

        public override CommandType CType => CommandType.Fun;

        public override string Parameters => "<temperature>";

        [Command("temp")]
        [Alias("temperature", "converttemp", "tempconvert", "convert")]
        [Summary("Converts temperatures!")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task HandleCommand(string temp)
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
                    Description = GetText(numbers, "°F", Unit.Fahrenheit),
                    Color = Color.DarkBlue
                }.Build());
            }
            else if (temp.EndsWith("C", StringComparison.OrdinalIgnoreCase) ||
                     temp.EndsWith("Celsius", StringComparison.OrdinalIgnoreCase))
            {
                await ReplyAsync(embed: new EmbedBuilder
                {
                    Title = "Converted Celsius",
                    Description = GetText(numbers, "°C", Unit.Celsius),
                    Color = Color.DarkBlue
                }.Build());
            }
            else if (temp.EndsWith("K", StringComparison.OrdinalIgnoreCase) ||
                     temp.EndsWith("Kelvin", StringComparison.OrdinalIgnoreCase))
            {
                await ReplyAsync(embed: new EmbedBuilder
                {
                    Title = "Converted Kelvin",
                    Description = GetText(numbers, "K", Unit.Kelvin),
                    Color = Color.DarkBlue
                }.Build());
            }
            else if (temp.EndsWith("Ra", StringComparison.OrdinalIgnoreCase) ||
                     temp.EndsWith("Rankine", StringComparison.OrdinalIgnoreCase))
            {
                await ReplyAsync(embed: new EmbedBuilder
                {
                    Title = "Converted Rankine",
                    Description = GetText(numbers, "°Ra", Unit.Rankine),
                    Color = Color.DarkBlue
                }.Build());
            }
            else if (temp.EndsWith("Re", StringComparison.OrdinalIgnoreCase) ||
                     temp.EndsWith("Reaumur", StringComparison.OrdinalIgnoreCase))
            {
                await ReplyAsync(embed: new EmbedBuilder
                {
                    Title = "Converted Réaumur",
                    Description = GetText(numbers, "°Re", Unit.Reaumur),
                    Color = Color.DarkBlue
                }.Build());
            }
            // If its any other unit than Celsius or Fahrenheit
            else
                await ReplyAsync(
                    embed: EmbedHelper.ErrorEmbed("Please specify a unit (F, C, K, Ra, Re, Fahrenheit, Celsius, Kelvin, Rankine, Reaumur)."));
        }

        private static string GetText(double temp, string unitCh, Unit unit) =>
            $"{temp}{unitCh} = {Math.Round(ToFahrenheit(temp, unit), 2)}°F" +
            $"\n{temp}{unitCh} = {Math.Round(ToCelsius(temp, unit), 2)}°C" +
            $"\n{temp}{unitCh} = {Math.Round(ToKelvin(temp, unit), 2)}K," +
            $"\n{temp}{unitCh} = {Math.Round(ToRankine(temp, unit), 2)}°Ra" +
            $"\n{temp}{unitCh} = {Math.Round(ToReaumur(temp, unit), 2)}°Re";

        public static double ToFahrenheit(double temp, Unit fromUnit)
        {
            return fromUnit switch
            {
                Unit.Fahrenheit => temp,
                Unit.Celsius => temp * 1.8 + 32,
                Unit.Kelvin => temp * 1.8 - 459.67,
                Unit.Rankine => temp - 459.67,
                Unit.Reaumur => temp * 2.25 + 32,
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }

        public static double ToCelsius(double temp, Unit fromUnit)
        {
            return fromUnit switch
            {
                Unit.Fahrenheit => (temp - 32) / 1.8,
                Unit.Celsius => temp,
                Unit.Kelvin => temp - 273.15,
                Unit.Rankine => (temp - 32 - 459.67) / 1.8,
                Unit.Reaumur => temp * 1.25,
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }

        public static double ToKelvin(double temp, Unit fromUnit)
        {
            return fromUnit switch
            {
                Unit.Fahrenheit => (temp + 459.67) / 1.8,
                Unit.Celsius => temp + 273.15,
                Unit.Kelvin => temp,
                Unit.Rankine => temp / 1.8,
                Unit.Reaumur => temp * 1.25 + 273.15,
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }

        public static double ToRankine(double temp, Unit fromUnit)
        {
            return fromUnit switch
            {
                Unit.Fahrenheit => temp + 459.67,
                Unit.Celsius => temp * 1.8 + 32 + 459.67,
                Unit.Kelvin => temp * 1.8,
                Unit.Rankine => temp,
                Unit.Reaumur => temp * 2.25 + 32 + 459.67,
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }

        public static double ToReaumur(double temp, Unit fromUnit)
        {
            return fromUnit switch
            {
                Unit.Fahrenheit => (temp - 32) / 2.25,
                Unit.Celsius => temp * 0.8,
                Unit.Kelvin => (temp - 273.15) * 0.8,
                Unit.Rankine => (temp - 32 - 459.67) / 2.25,
                Unit.Reaumur => temp,
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }
    }
}