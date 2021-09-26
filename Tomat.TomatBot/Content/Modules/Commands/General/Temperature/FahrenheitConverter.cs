#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;

namespace Tomat.TomatBot.Content.Modules.Commands.General.Temperature
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