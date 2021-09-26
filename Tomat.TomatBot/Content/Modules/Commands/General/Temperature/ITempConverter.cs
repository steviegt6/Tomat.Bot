#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

namespace Tomat.TomatBot.Content.Modules.Commands.General.Temperature
{
    public interface ITempConverter
    {
        TempUnit AssociatedUnit { get; }

        string LongName { get; }

        string ShortName { get; }

        string TextRepresentation { get; }

        double ToTemp(double temperature, TempUnit originalUnit);
    }
}