#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using Discord.WebSocket;
using Tomat.TomatBot.Core.Bot;

namespace Tomat.TomatBot.Core.Services
{
    public interface IService
    {
        IServiceProvider ServiceProvider { get; }

        DiscordSocketClient Client { get; }

        DiscordBot Bot { get; }
    }
}