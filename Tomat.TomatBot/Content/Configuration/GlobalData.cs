#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System.Collections.Generic;

namespace Tomat.TomatBot.Content.Configuration
{
    public sealed class GlobalData
    {
        public Dictionary<string, (int, int)> Ratings;

        public GlobalData()
        {
            Ratings = new Dictionary<string, (int, int)>();
        }
    }
}