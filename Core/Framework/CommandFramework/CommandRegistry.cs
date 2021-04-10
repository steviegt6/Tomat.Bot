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
            Logger.Info("Initializing commands...");

            helpCommandData = new List<HelpCommandData>();
            infoCommands = new List<string>();
            funCommands = new List<string>();

            Logger.Info("Loading commands from attribute data...");

            foreach (Type type in typeof(CommandRegistry).Assembly.GetTypes().Where(
                x => x.IsSubclassOf(typeof(TomatCommand)) && !x.IsAbstract && x.GetConstructor(new Type[0]) != null))
                if (Activator.CreateInstance(type) is TomatCommand command)
                {
                    Logger.Info($"Found command to register: {command.Name}");

                    string helpEntry = $"`{command.Name}";

                    if (command.Parameters != null)
                        helpEntry += command.Parameters;

                    helpEntry += $"` - {command.CommandSummary}";

                    switch (command.CType)
                    {
                        case CommandType.Info:
                            infoCommands.Add(helpEntry);
                            break;

                        case CommandType.Fun:
                            funCommands.Add(helpEntry);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    helpCommandData.Add(command.HelpData);
                }

            Logger.Info("Finished loading commands from attribute data!");
            Logger.Info("Initialized commands!");
        }
    }
}