#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;

namespace Tomat.TomatBot.Content.Commands.Modules.Temperature
{
    public class KelvinConverter : ITempConverter
    {
        public TempUnit AssociatedUnit => TempUnit.Kelvin;

        public string LongName => "Kelvin";

        public string ShortName => "K";

        public string TextRepresentation => "K";

        public double ToTemp(double temperature, TempUnit originalUnit)
        {
            return originalUnit switch
            {
                TempUnit.Fahrenheit => (temperature + 459.67) / 1.8,
                TempUnit.Celsius => temperature + 273.15,
                TempUnit.Kelvin => temperature,
                TempUnit.Rankine => temperature / 1.8,
                TempUnit.Reaumur => temperature * 1.25 + 273.15,
                _ => throw new ArgumentOutOfRangeException(nameof(originalUnit), originalUnit, null)
            };
        }
    }
}