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
            Rankine
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
            else if (temp.EndsWith("R", StringComparison.OrdinalIgnoreCase) ||
                     temp.EndsWith("Rankine", StringComparison.OrdinalIgnoreCase))
            {
                await ReplyAsync(embed: new EmbedBuilder
                {
                    Title = "Converted Rankine",
                    Description = GetText(numbers, "°R", Unit.Rankine),
                    Color = Color.DarkBlue
                }.Build());
            }
            // If its any other unit than Celsius or Fahrenheit
            else
                await ReplyAsync(
                    embed: EmbedHelper.ErrorEmbed("Please specify a unit (F, C, K, R, Fahrenheit, Celsius, Kelvin, Rankine)."));
        }

        private static string GetText(double temp, string the, Unit unit) =>
            $"{temp}{the} = {ToFahrenheit(temp, unit)}°F" +
            $"\n{temp}{the} = {ToCelsius(temp, unit)}°C" +
            $"\n{temp}{the} = {ToKelvin(temp, unit)}°K," +
            $"\n{temp}{the} = {ToRankine(temp, unit)}°R";

        public static double ToFahrenheit(double temp, Unit fromUnit)
        {
            return fromUnit switch
            {
                Unit.Fahrenheit => temp,
                Unit.Celsius => Math.Round(temp * 9 / 5 + 32, 2),
                Unit.Kelvin => Math.Round((temp - 273.15) * 1.8 + 32, 2),
                Unit.Rankine => temp - 491.67 + 32,
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }

        public static double ToCelsius(double temp, Unit fromUnit)
        {
            return fromUnit switch
            {
                Unit.Fahrenheit => Math.Round((temp - 32) / 1.8, 2),
                Unit.Celsius => temp,
                Unit.Kelvin => temp + 273.15,
                Unit.Rankine => Math.Round((temp - 491.67) / 1.8, 2),
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }

        public static double ToKelvin(double temp, Unit fromUnit)
        {
            return fromUnit switch
            {
                Unit.Fahrenheit => Math.Round((temp - 32) / 1.8 + 273.15, 2),
                Unit.Celsius => temp - 273.15,
                Unit.Kelvin => temp,
                Unit.Rankine => Math.Round((temp - 491.67) / 1.8 + 273.15, 2),
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }

        public static double ToRankine(double temp, Unit fromUnit)
        {
            return fromUnit switch
            {
                Unit.Fahrenheit => temp - 32 + 491.67,
                Unit.Celsius => Math.Round(temp * 1.8 + 491.67, 2),
                Unit.Kelvin => Math.Round((temp - 273.15) * 1.8, 2) + 491.67,
                Unit.Rankine => temp,
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }
    }
}