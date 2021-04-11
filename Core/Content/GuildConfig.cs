using Newtonsoft.Json;

namespace TomatBot.Core.Content
{
    public sealed class GuildConfig
    {
        [JsonIgnore]
        public ulong AssociatedId;
    }
}
