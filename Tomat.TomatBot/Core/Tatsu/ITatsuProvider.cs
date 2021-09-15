#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using Tomat.TatsuSharp;

namespace Tomat.TomatBot.Core.Tatsu
{
    public interface ITatsuProvider
    {
        TatsuClient TatsuClient { get; }

        // API key
        string TatsuToken { get; }
    }
}