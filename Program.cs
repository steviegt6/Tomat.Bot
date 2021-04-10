using TomatBot.Core;
using TomatBot.Core.Framework.CommandFramework;

// Register command information
CommandRegistry.LoadCommandEntries();

// Start up the bot
new BotStartup().StartBotAsync().GetAwaiter().GetResult();