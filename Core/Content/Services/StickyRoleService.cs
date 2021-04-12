using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using TomatBot.Core.Framework.ServiceFramework;
using TomatBot.Core.Logging;

namespace TomatBot.Core.Content.Services
{
    public sealed class StickyRoleService : ServiceBase
    {
        // TODO: make configurable
        public StickyRoleService(IServiceProvider services) : base(services) { }

        public override async Task Initialize()
        {
            Client.UserLeft += SaveUserRoles; 
            Client.UserJoined += ApplySavedUserRoles;
            await LoggerService.TaskLog(new LogMessage(LogSeverity.Debug, "Service", "Initialized StickyRoleService!"));
        }

        private async Task SaveUserRoles(SocketGuildUser user)
        {
            // TODO: make bot saving configurable, add config for list of users to ignore, add config for list of roles to ignore
            if (user.IsBot || user.IsWebhook)
                return;

            List<IRole> roles = new();
        }

        private async Task ApplySavedUserRoles(SocketGuildUser user)
        {
            // TODO: make bot saving configurable, add config for list of users to ignore, add config for list of roles to ignore
            if (user.IsBot || user.IsWebhook || !user.Roles.Any())
                return;
        }
    }
}