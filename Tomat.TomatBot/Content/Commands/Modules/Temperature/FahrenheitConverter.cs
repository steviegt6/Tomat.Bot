#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;

namespace Tomat.TomatBot.Content.Commands.Modules.Temperature
{
    public class FahrenheitConverter : ITempConverter
    {
        public TempUnit AssociatedUnit => TempUnit.Fahrenheit;

        public string LongName => "Fahrenheit";

        public string ShortName => "F";

        public string TextRepresentation => "°F";

        public double ToTemp(double temperature, TempUnit originalUnit)
        {
            return originalUnit switch
            {
                TempUnit.Fahrenheit => temperature,
                TempUnit.Celsius => temperature * 1.8 + 32,
                TempUnit.Kelvin => temperature * 1.8 - 459.67,
                TempUnit.Rankine => temperature - 459.67,
                TempUnit.Reaumur => temperature * 2.25 + 32,
                _ => throw new ArgumentOutOfRangeException(nameof(originalUnit), originalUnit, null)
            };
        }
    }
}