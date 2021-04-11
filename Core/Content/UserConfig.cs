using Newtonsoft.Json;

namespace TomatBot.Core.Content
{
    public sealed class UserConfig
    {
        [JsonIgnore]
        public ulong AssociatedId;
    }
}
