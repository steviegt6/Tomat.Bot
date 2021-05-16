using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Tomat.Conveniency.Embeds;
using Tomat.Logging;
using Tomat.TomatBot.Common;
using Tomat.TomatBot.Utilities;

namespace Tomat.TomatBot
{
    public class CommandHandler
    {
        public static class Registry
        {
            public sealed class ModuleData
            {
                public struct CommandData
                {
                    public string commandName;
                    public string[] commandAliases;
                    public string commandParameters;
                    public string commandDescription;

                    public CommandData(string commandName, string[] commandAliases, string commandParameters,
                        string commandDescription)
                    {
                        this.commandName = commandName;
                        this.commandAliases = commandAliases;
                        this.commandParameters = commandParameters;
                        this.commandDescription = commandDescription;
                    }

                    public override string ToString() => commandName;
                }

                public string displayName;
                public List<CommandData> commands;

                public ModuleData(string displayName, List<CommandData> commands)
                {
                    this.displayName = displayName;
                    this.commands = commands;
                }
            }

            public static List<ModuleData> Modules = new();

            public static void Load()
            {
                Logger.Debug("Loading commands from attribute data...");

                foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    try
                    {
                        if (Activator.CreateInstance(type) is not ModuleBase<SocketCommandContext>)
                            continue;

                        ModuleInfoAttribute? info = type.GetCustomAttribute<ModuleInfoAttribute>();

                        if (info is null || !info.ShouldDisplay)
                            continue;

                        List<ModuleData.CommandData> commands = (from methodInfo in type.GetMethods()
                            let command = methodInfo.GetCustomAttribute<CommandAttribute>()?.Text ?? ""
                            let aliases =
                                methodInfo.GetCustomAttribute<AliasAttribute>()?.Aliases ?? Array.Empty<string>()
                            let description = methodInfo.GetCustomAttribute<SummaryAttribute>()?.Text ?? ""
                            let parameters = methodInfo.GetCustomAttribute<ParametersAttribute>()?.Parameters ?? ""
                            where command != ""
                            select new ModuleData.CommandData(command, aliases, parameters, description)).ToList();

                        Modules.Add(new ModuleData(info.DisplayName, commands));
                    }
                    catch
                    {
                        // ignore
                    }
                }

                Logger.Debug("Loaded commands from attribute data!");

            }
        }

        public DiscordSocketClient Client { get; }

        public CommandService Commands { get; }

        public CommandHandler()
        {
            Client = BotStartup.Client;
            Commands = BotStartup.Provider.GetRequiredService<CommandService>();
            InstallCommandsAsync().GetAwaiter().GetResult();
        }

        internal async Task InstallCommandsAsync()
        {
            // Hook a method to the MessageReceived event, allowing us to detect and respond to messages
            Client.MessageReceived += HandleCommandAsync;
            Commands.CommandExecuted += HandleError;

            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        private async Task HandleCommandAsync(SocketMessage message)
        {
            if (message.ValidateMessageMention(out CommandUtilities.InvalidMessageReason invalidReason, out int argPos,
                Client))
                await Commands.ExecuteAsync(new SocketCommandContext(Client, message as SocketUserMessage), argPos,
                    null);

            if (CommandUtilities.IsFatalReason(invalidReason))
            {
                BaseEmbed embed = new(message.Author)
                {
                    Title = $"Validation error encountered: {invalidReason}",
                    Description =
                        "This likely isn't much of an issue. If you believe it is, report to the developers immediately!"
                };

                await message.Channel.SendMessageAsync(embed: embed.Build());
            }
        }

        private static async Task HandleError(Optional<CommandInfo> info, ICommandContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                BaseEmbed embed = new(context.User)
                {
                    Title = $"Error encountered: {result.Error}",
                    Description = result.ErrorReason
                };

                await context.Channel.SendMessageAsync(embed: embed.Build());
            }
        }
    }
}