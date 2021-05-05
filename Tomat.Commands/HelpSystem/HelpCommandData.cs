namespace Tomat.CommandFramework.HelpSystem
{
    /// <summary>
    /// A struct containing the name of a command and its description.
    /// </summary>
    public readonly struct HelpCommandData
    {
        public readonly string command;
        public readonly string description;
        public readonly string? parameters;
        public readonly string[]? aliases;

        public HelpCommandData(string command, string description, string? parameters = null, string[]? aliases = null)
        {
            this.command = command;
            this.description = description;
            this.parameters = parameters;
            this.aliases = aliases;
        }
    }
}