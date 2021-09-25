#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using Discord.Commands;
using Discord.WebSocket;
using Tomat.TomatBot.Core.Bot;

namespace Tomat.TomatBot.Core.CommandContext
{
    public class BotCommandContext : SocketCommandContext
    {
        public DiscordBot Bot { get; }

        public BotCommandContext(DiscordBot bot, DiscordSocketClient client, SocketUserMessage msg) : base(client, msg)
        {
            Bot = bot;
        }
    }
}