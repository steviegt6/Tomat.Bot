#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

namespace Tomat.TomatBot.Content.Commands.Modules.Temperature
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