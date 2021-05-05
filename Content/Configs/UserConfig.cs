namespace TomatBot.Content.Configs
{
    public sealed class UserConfig
    {
        public ulong AssociatedId { get; }

        public uint Level { get; }
        
        public uint Experience { get; }

        public UserConfig()
        {
            AssociatedId = 0;
            Level = 0;
            Experience = 0;
        }
    }
}
