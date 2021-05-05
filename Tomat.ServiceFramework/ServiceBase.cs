using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Tomat.ServiceFramework
{
    public abstract class ServiceBase
    {
        public IServiceProvider Services { get; protected set; }

        public DiscordSocketClient Client { get; protected set; }

        protected ServiceBase(IServiceProvider services)
        {
            Services = services;
            Client = services.GetRequiredService<DiscordSocketClient>();
        }
    }
}
