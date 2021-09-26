#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System.Collections.Generic;

namespace Tomat.TomatBot.Core.Services.Commands
{
    public readonly struct ModuleData
    {
        public readonly string DisplayName;
        public readonly List<CommandData> Commands;

        public ModuleData(string displayName, List<CommandData> commands)
        {
            DisplayName = displayName;
            Commands = commands;
        }
    }
}