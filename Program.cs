using Tomat.CommandFramework;
using TomatBot;

// Register bot command information before official bot startup
// (potential) TODO: start up bot, and change status according to what it's currently doing!
CommandRegistry.LoadCommandEntries();

// Start up the bot
BotStartup.StartBotAsync()
    .GetAwaiter()
    .GetResult();
