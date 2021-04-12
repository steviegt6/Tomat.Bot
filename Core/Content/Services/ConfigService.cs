using System;
using System.IO;
using Newtonsoft.Json;
using TomatBot.Core.Content.Configs;
using TomatBot.Core.Framework.ServiceFramework;

namespace TomatBot.Core.Content.Services
{
    // TODO: User configs
    public sealed class ConfigService : ServiceBase
    {
        public BotConfig Config { get; }

        public static string ConfigPath => "Configs";

        public static JsonSerializerSettings SerializationSettings { get; private set; } = null!;

        public ConfigService(IServiceProvider services) : base(services)
        {
            Directory.CreateDirectory(ConfigPath);

            SerializationSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            Config = new BotConfig();
        }
    }
}
