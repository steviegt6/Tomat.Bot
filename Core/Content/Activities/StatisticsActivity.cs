using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;

namespace TomatBot.Core.Content.Activities
{
    /// <summary>
    ///     Basic <see cref="IActivity"/> that displays the total amount of servers the bot is in, along with the total members of each server.
    /// </summary>
    public class StatisticsActivity : IActivity
    {
        public string Name
        {
            get
            {
                IReadOnlyCollection<SocketGuild> guilds = BotStartup.Client.Guilds;

                // TODO: Make it so we don't count duplicate members? (check IDs)
                int totalUserCount = guilds.Sum(guild => guild.MemberCount);

                return $"{guilds.Count} guilds | Watching {totalUserCount} users";
            }
        }

        public ActivityType Type => ActivityType.Watching;

        public ActivityProperties Flags => ActivityProperties.None;

        public string Details => "Analyzing statics.";
    }
}