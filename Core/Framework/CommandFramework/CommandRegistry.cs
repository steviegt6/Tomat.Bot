using System;
using System.Collections.Generic;
using System.Linq;
using Discord.Commands;
using TomatBot.Core.Framework.DataStructures;
using TomatBot.Core.Logging;

namespace TomatBot.Core.Framework.CommandFramework
{
    public class CommandRegistry : ModuleBase<SocketCommandContext>
    {
        internal static List<HelpCommandData>? helpCommandData;
        internal static List<string>? infoCommands;
        internal static List<string>? funCommands;

        internal static void LoadCommandEntries()
        {
            LoggerService.Debug("Initializing commands...");

            helpCommandData = new List<HelpCommandData>();
            infoCommands = new List<string>();
            funCommands = new List<string>();

            LoggerService.Debug("Loading commands from attribute data...");

            foreach (Type type in typeof(CommandRegistry).Assembly.GetTypes().Where(
                x => x.IsSubclassOf(typeof(TomatCommand)) && !x.IsAbstract && x.GetConstructor(Array.Empty<Type>()) != null))
                if (Activator.CreateInstance(type) is TomatCommand command)
                {
                    LoggerService.Debug($"Found command to register: {command.Name}");

                    string helpEntry = $"`{command.Name}";

                    if (command.Parameters != null)
                        helpEntry += $" {command.Parameters}";

                    helpEntry += $"` - {command.CommandSummary}";

                    switch (command.CType)
                    {
                        case CommandType.Info:
                            infoCommands.Add(helpEntry);
                            break;

                        case CommandType.Fun:
                            funCommands.Add(helpEntry);
                            break;

                        case CommandType.Hidden:
                            LoggerService.Debug($"Skipping registration of hidden command: {command.Name}");
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    helpCommandData.Add(new HelpCommandData(command.HelpData.command, command.HelpData.description, command.Parameters, command.Aliases));
                }

            LoggerService.Debug("Finished loading commands from attribute data!");
            LoggerService.Debug("Initialized commands!");
        }
    }
}