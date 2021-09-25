#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
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
using Tomat.TomatBot.Content.Commands.Modules.Temperature;
using Tomat.TomatBot.Core.CommandContext;
using Tomat.TomatBot.Core.Services.Commands;

namespace Tomat.TomatBot.Content.Commands.Modules
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
        [Parameters("<temperature> (symbol suffix)")]
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
                double rounded = Math.Round(TemperatureConverters[newUnit].ToTemp(temp, converter.AssociatedUnit));

                builder.AppendLine($"{left} = {rounded}");
            }

            foreach (TempUnit tUnit in TemperatureConverters.Keys)
                AddUnit(tUnit);


            return builder.ToString();
        }

        #endregion
    }
}