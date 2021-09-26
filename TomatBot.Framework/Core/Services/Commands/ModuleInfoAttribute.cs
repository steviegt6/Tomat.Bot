#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;

namespace Tomat.TomatBot.Core.Services.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ModuleInfoAttribute : Attribute
    {
        public string DisplayName { get; }

        public bool ShouldDisplay { get; }

        public ModuleInfoAttribute(string displayName, bool shouldDisplay = true)
        {
            DisplayName = displayName;
            ShouldDisplay = shouldDisplay;
        }
    }
}