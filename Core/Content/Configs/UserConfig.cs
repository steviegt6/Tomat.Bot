using Newtonsoft.Json;

namespace TomatBot.Core.Content.Configs
{
    public sealed class UserConfig
    {
        [JsonIgnore]
        public ulong AssociatedId { get; internal set; }

        // public uint Prestige { get; }

        public uint Level { get; }
        
        public uint Experience { get; }

        [JsonConstructor]
        public UserConfig(ulong id)
        {
            AssociatedId = id;
            // Prestige = 0;
            Level = 0;
            Experience = 0;
        }
    }
}
