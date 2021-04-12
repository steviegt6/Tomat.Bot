using System.Collections.Generic;

namespace TomatBot.Core.Content.Configs
{
    public sealed class GuildConfig
    {
        public ulong associatedId;
        public string guildPrefix;

        /// <summary>
        /// (dev notes)
        /// key -> user id
        /// uint1 -> prestiges
        /// uint2 -> levels
        /// uint3 -> experience
        /// </summary>
        public Dictionary<ulong, (uint, uint, uint)> levelData;
        
        public GuildConfig()
        {
            associatedId = 0;
            guildPrefix = "tomat!";
            levelData = new Dictionary<ulong, (uint, uint, uint)>();
        }
    }
}
