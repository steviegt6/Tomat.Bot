#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System.Threading.Tasks;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Tomat.TomatBot.Common.Logging;
using Tomat.TomatBot.Core.Bot;

namespace Tomat.TomatBot.Core.Services
{
    public static class ServiceRegistry
    {
        public static async Task InitializeServicesAsync(DiscordBot discordDiscordBot)
        {
            LogMessage log = new LogBuilder().WithMessage("Initializing services!").WithSource("Start-Up").Build();

            await discordDiscordBot.LogMessageAsync(log);

            foreach (ServiceDescriptor descriptor in discordDiscordBot.Services)
            foreach (object? service in discordDiscordBot.ServiceProvider.GetServices(descriptor.ServiceType))
            {
                if (service is not IInitializableService initializableService)
                    continue;

                await initializableService.InitializeAsync();
            }
        }
    }
}