using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace TomatBot.Core.Framework.ServiceFramework
{
    public abstract class ServiceBase
    {
        protected IServiceProvider Services { get; }
        protected DiscordSocketClient Client { get; }

        protected ServiceBase(IServiceProvider services)
        {
            Services = services;
            Client = services.GetRequiredService<DiscordSocketClient>();
        }

        public virtual async Task Initialize() => await Task.CompletedTask;
    }
}
