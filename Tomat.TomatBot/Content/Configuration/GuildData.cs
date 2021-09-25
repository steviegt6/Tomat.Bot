#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

namespace Tomat.TomatBot.Content.Configuration
{
    public sealed class GuildData
    {
        public ulong AssociatedId;
        public string GuildPrefix;

        public GuildData()
        {
            AssociatedId = 0UL;
            GuildPrefix = "";
        }
    }
}