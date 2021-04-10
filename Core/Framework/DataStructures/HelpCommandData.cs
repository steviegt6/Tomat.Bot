namespace TomatBot.Core.Framework.DataStructures
{
    /// <summary>
    /// A struct containing the name of a command and its description.
    /// </summary>
    public readonly struct HelpCommandData
    {
        public readonly string command;
        public readonly string description;

        public HelpCommandData(string command, string description)
        {
            this.command = command;
            this.description = description;
        }

        public override string ToString() => $"{command}: {description}";
    }
}