using System.Collections.Generic;

namespace TomatBot.Core.Content.Configs
{
    public sealed class GlobalConfig
    {
        public Dictionary<string, (int, int)> ratings;

        public GlobalConfig() => ratings = new Dictionary<string, (int, int)>();
    }
}
