using Tomat.TomatBot;

CommandHandler.Registry.Load();

// Start up the bot
BotStartup.StartBotAsync()
    .GetAwaiter()
    .GetResult();
