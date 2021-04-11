using System;
using System.Threading.Tasks;
using TomatBot.Core.Framework.ServiceFramework;

namespace TomatBot.Core.Logging
{
    public sealed partial class LoggerService : ServiceBase
    {
        public LoggerService(IServiceProvider services) : base(services) => Client.Log += TaskLog;

        public override async Task Initialize()
        {
            // TODO: Logging once we create a config system for per-guild options
            await base.Initialize();
            /*Client.ChannelCreated += LogChannelCreation;
            Client.ChannelDestroyed += ClientOnChannelDestroyed;
            Client.ChannelUpdated += ClientOnChannelUpdated;

            Client.GuildMemberUpdated += ClientOnGuildMemberUpdated;
            Client.GuildUpdated += ClientOnGuildUpdated;
            Client.UserJoined += ClientOnUserJoined;
            Client.UserLeft += ClientOnUserLeft;
            Client.UserBanned += ClientOnUserBanned;
            Client.UserUnbanned += ClientOnUserUnbanned;
            Client.UserUpdated += ClientOnUserUpdated;

            Client.InviteCreated += ClientOnInviteCreated;
            Client.InviteDeleted += ClientOnInviteDeleted;

            Client.MessageDeleted += ClientOnMessageDeleted;
            Client.MessageUpdated += ClientOnMessageUpdated;
            Client.MessagesBulkDeleted += ClientOnMessagesBulkDeleted;*/
        }
    }
}
