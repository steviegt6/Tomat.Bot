using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Discord.Commands;
using Tomat.CommandFramework.HelpSystem;
using Tomat.Logging;

namespace Tomat.CommandFramework
{
    public class CommandRegistry : ModuleBase<SocketCommandContext>
    {
        public static List<HelpCommandData> CommandData { get; set; } = new();

        public static List<string> InfoCommands { get; set; } = new();

        public static List<string> FunCommands { get; set; } = new();

        public static List<string> ConfigCommands { get; set; } = new();

        public static void LoadCommandEntries()
        {
            Logger.Debug("Loading commands from attribute data...");

            foreach (Type type in Assembly.GetCallingAssembly().GetTypes().Where(x =>
                x.IsSubclassOf(typeof(BaseCommand)) && !x.IsAbstract && x.GetConstructor(Array.Empty<Type>()) != null))
                if (Activator.CreateInstance(type) is BaseCommand command)
                {
                    Logger.Debug($"Found command to register: {command.Name}");

                    string helpEntry = $"`{command.Name}";

                    if (command.Parameters != null)
                        helpEntry += $" {command.Parameters}";

                    RegisterCommand(helpEntry + $"` - {command.CommandSummary}", command);

                    CommandData.Add(new HelpCommandData(command.HelpData.command,
                        command.HelpData.description,
                        command.Parameters,
                        command.Aliases));
                }

            Logger.Debug("Finished loading commands from attribute data!");
        }

        public static void RegisterCommand(string entry, BaseCommand command)
        {
            switch (command.CType)
            {
                case CommandType.Info:
                    InfoCommands.Add(entry);
                    break;

                case CommandType.Fun:
                    FunCommands.Add(entry);
                    break;

                case CommandType.Hidden:
                    Logger.Debug($"Skipping registration of hidden command: {command.Name}");
                    break;

                case CommandType.Configuration:
                    ConfigCommands.Add(entry);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(command));
            }
        }
    }
}