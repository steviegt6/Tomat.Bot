#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;

namespace Tomat.TomatBot.Content.Commands.Modules.Temperature
{
    public class CelsiusConverter : ITempConverter
    {
        public TempUnit AssociatedUnit => TempUnit.Celsius;

        public string LongName => "Celsius";

        public string ShortName => "C";

        public string TextRepresentation => "°C";

        public double ToTemp(double temperature, TempUnit originalUnit)
        {
            return originalUnit switch
            {
                TempUnit.Fahrenheit => (temperature - 32) / 1.8,
                TempUnit.Celsius => temperature,
                TempUnit.Kelvin => temperature - 273.15,
                TempUnit.Rankine => (temperature - 459.67 - 32) / 1.8,
                TempUnit.Reaumur => temperature * 1.25,
                _ => throw new ArgumentOutOfRangeException(nameof(originalUnit), originalUnit, null)
            };
        }
    }
}