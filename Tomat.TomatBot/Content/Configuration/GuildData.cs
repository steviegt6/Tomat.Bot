#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
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