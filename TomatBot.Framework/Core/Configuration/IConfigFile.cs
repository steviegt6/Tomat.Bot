#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using Tomat.TomatBot.Core.Bot;

namespace Tomat.TomatBot.Core.Configuration
{
    public interface IConfigFile : IConfigurable
    {
        DiscordBot Bot { get; }

        string SavePath { get; }
    }
}