#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;

namespace Tomat.TomatBot.Content.Commands.Modules.Temperature
{
    public class RankineConverter : ITempConverter
    {
        public TempUnit AssociatedUnit => TempUnit.Rankine;

        public string LongName => "Rankine";

        public string ShortName => "Ra";

        public string TextRepresentation => "°Ra";

        public double ToTemp(double temperature, TempUnit originalUnit)
        {
            return originalUnit switch
            {
                TempUnit.Fahrenheit => temperature + 459.67,
                TempUnit.Celsius => temperature * 1.8 + 32 + 459.67,
                TempUnit.Kelvin => temperature * 1.8,
                TempUnit.Rankine => temperature,
                TempUnit.Reaumur => temperature * 2.25 + 32 + 459.67,
                _ => throw new ArgumentOutOfRangeException(nameof(originalUnit), originalUnit, null)
            };
        }

    }
}