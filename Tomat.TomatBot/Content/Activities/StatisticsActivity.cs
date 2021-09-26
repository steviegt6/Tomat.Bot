#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using Tomat.Framework.Common.Activities;

namespace Tomat.TomatBot.Content.Activities
{
    public class StatisticsActivity : Activity
    {
        public virtual IReadOnlyCollection<SocketGuild> Guilds { get; }

        public override string Name
        {
            get
            {
                int totalUserCount = Guilds.Sum(x => x.MemberCount);

                return $"{totalUserCount} users in {Guilds.Count} guilds";
            }
        }

        public override string Details => "Analyzing statistics.";

        public StatisticsActivity(IReadOnlyCollection<SocketGuild> guilds, ActivityType type = ActivityType.Watching,
            ActivityProperties flags = ActivityProperties.None) : base(type: type, flags: flags)
        {
            Guilds = guilds;
        }
    }
}