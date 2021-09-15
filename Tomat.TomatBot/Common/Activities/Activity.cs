#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using Discord;

namespace Tomat.TomatBot.Common.Activities
{
    public class Activity : IActivity
    {
        public Activity(string name = "", ActivityType type = ActivityType.Playing,
            ActivityProperties flags = ActivityProperties.None, string details = "")
        {
            Name = name;
            Type = type;
            Flags = flags;
            Details = details;
        }

        public virtual string Name { get; }

        public virtual ActivityType Type { get; }

        public virtual ActivityProperties Flags { get; }

        public virtual string Details { get; }
    }
}