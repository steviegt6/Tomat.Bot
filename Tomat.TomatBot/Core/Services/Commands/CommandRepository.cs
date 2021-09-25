#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Tomat.TomatBot.Common.Logging;
using Tomat.TomatBot.Core.Bot;
using Tomat.TomatBot.Core.CommandContext;

namespace Tomat.TomatBot.Core.Services.Commands
{
    public class CommandRepository
    {
        public List<ModuleData> Modules { get; } = new();

        public async Task RegisterFromBot(DiscordBot bot)
        {
            foreach (Assembly assembly in bot.Assemblies)
            {
                await bot.LogMessageAsync(new LogBuilder().WithSource("Commands")
                    .WithMessage($"Registering commands from assembly: {assembly.FullName}").Build());

                RegisterFromAssembly(assembly);
            }

            await Task.CompletedTask;
        }

        public void RegisterFromAssembly(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                try
                {
                    if (Activator.CreateInstance(type) is not ModuleBase<BotCommandContext>)
                        continue;

                    ModuleInfoAttribute? info = type.GetCustomAttribute<ModuleInfoAttribute>();

                    if (info is null || !info.ShouldDisplay)
                        continue;

                    List<CommandData> commands = (from methodInfo in type.GetMethods()
                        let command = methodInfo.GetCustomAttribute<CommandAttribute>()?.Text ?? ""
                        let aliases = methodInfo.GetCustomAttribute<AliasAttribute>()?.Aliases ?? Array.Empty<string>()
                        let description = methodInfo.GetCustomAttribute<SummaryAttribute>()?.Text ?? ""
                        let parameters = methodInfo.GetCustomAttribute<ParametersAttribute>()?.Parameters ?? ""
                        where command != ""
                        select new CommandData(command, aliases, parameters, description)).ToList();

                    Modules.Add(new ModuleData(info.DisplayName, commands));
                }
                catch
                {
                    // ignore
                }
            }
        }
    }
}