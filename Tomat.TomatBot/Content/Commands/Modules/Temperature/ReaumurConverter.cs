#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;

namespace Tomat.TomatBot.Content.Commands.Modules.Temperature
{
    public class ReaumurConverter : ITempConverter
    {
        public TempUnit AssociatedUnit => TempUnit.Reaumur;

        public string LongName => "Réaumur";

        public string ShortName => "Re";

        public string TextRepresentation => "°Re";

        public double ToTemp(double temperature, TempUnit originalUnit)
        {
            return originalUnit switch
            {
                TempUnit.Fahrenheit => (temperature - 32) / 2.25,
                TempUnit.Celsius => temperature * 0.8,
                TempUnit.Kelvin => (temperature - 273.15) * 0.8,
                TempUnit.Rankine => (temperature - 32 - 459.67) / 2.25,
                TempUnit.Reaumur => temperature,
                _ => throw new ArgumentOutOfRangeException(nameof(originalUnit), originalUnit, null)
            };
        }
    }
}