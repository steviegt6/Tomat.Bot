using System;

namespace Tomat.TomatBot.Common 
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
