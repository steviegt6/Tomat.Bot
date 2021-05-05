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
            Kelvin
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
                double toF = ToFahrenheit(numbers, Unit.Fahrenheit);
                double toC = ToCelsius(numbers, Unit.Fahrenheit);
                double toK = ToKelvin(numbers, Unit.Fahrenheit);

                await ReplyAsync(embed: new EmbedBuilder
                {
                    Title = "Converted Celsius",
                    Description = $"{numbers}°F = {toF}°F" +
                                  $"\n{numbers}°F = {toC}°C" +
                                  $"\n{numbers}°F = {toK}°K",
                    Color = Color.DarkBlue
                }.Build());
            }
            else if (temp.EndsWith("C", StringComparison.OrdinalIgnoreCase) ||
                     temp.EndsWith("Celsius", StringComparison.OrdinalIgnoreCase))
            {
                double toF = ToFahrenheit(numbers, Unit.Celsius);
                double toC = ToCelsius(numbers, Unit.Celsius);
                double toK = ToKelvin(numbers, Unit.Celsius);

                await ReplyAsync(embed: new EmbedBuilder
                {
                    Title = "Converted Celsius",
                    Description = $"{numbers}°C = {toF}°F" +
                                  $"\n{numbers}°C = {toC}°C" +
                                  $"\n{numbers}°C = {toK}°K",
                    Color = Color.DarkBlue
                }.Build());
            }
            else if (temp.EndsWith("K", StringComparison.OrdinalIgnoreCase) ||
                     temp.EndsWith("Kelvin", StringComparison.OrdinalIgnoreCase))
            {
                double toF = ToFahrenheit(numbers, Unit.Kelvin);
                double toC = ToCelsius(numbers, Unit.Kelvin);
                double toK = ToKelvin(numbers, Unit.Kelvin);

                await ReplyAsync(embed: new EmbedBuilder
                {
                    Title = "Converted Celsius",
                    Description = $"{numbers}°K = {toF}°F" +
                                  $"\n{numbers}°K = {toC}°C" +
                                  $"\n{numbers}°K = {toK}°K",
                    Color = Color.DarkBlue
                }.Build());
            }
            // If its any other unit than Celsius or Fahrenheit
            else
                await ReplyAsync(
                    embed: EmbedHelper.ErrorEmbed("Please specify a unit (F, C, K, Fahrenheit, Celsius, Kelvin)."));
        }

        public static double ToFahrenheit(double temp, Unit fromUnit)
        {
            return fromUnit switch
            {
                Unit.Fahrenheit => temp,
                Unit.Celsius => Math.Round(temp * 9 / 5 + 32, 2),
                Unit.Kelvin => Math.Round((temp - 273.15) * 1.8 + 32),
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }

        public static double ToCelsius(double temp, Unit fromUnit)
        {
            return fromUnit switch
            {
                Unit.Fahrenheit => Math.Round((temp - 32) / 1.8),
                Unit.Celsius => temp,
                Unit.Kelvin => temp + 273.15,
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }

        public static double ToKelvin(double temp, Unit fromUnit)
        {
            return fromUnit switch
            {
                Unit.Fahrenheit => Math.Round((temp - 32) / 1.8 + 273.15),
                Unit.Celsius => temp - 273.15,
                Unit.Kelvin => temp,
                _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null)
            };
        }
    }
}