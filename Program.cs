using TomatBot.Core;
using TomatBot.Core.Framework.CommandFramework;

namespace TomatBot
{
    public static class Program
    {
        /// <summary>
        ///     Main entry-point.
        /// </summary>
        public static void Main()
        {
            // Register command information
            CommandRegistry.LoadCommandEntries();

            // Start up the bot
            BotStartup.StartBotAsync()
                .GetAwaiter()
                .GetResult();
        }
    }
}