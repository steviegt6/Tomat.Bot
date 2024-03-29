﻿#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System.IO;
using Tomat.Framework.Core.Bot;
using Tomat.TomatBot.Content;

string token = await File.ReadAllTextAsync("token.txt");
string tatsu = await File.ReadAllTextAsync("tatsu.txt");
string lvlnk = await File.ReadAllTextAsync("lvlnk.txt");

token = token.Trim();
tatsu = tatsu.Trim();
lvlnk = lvlnk.Trim();

using DiscordBot discordBot = new TomatBot(token, tatsu, lvlnk);
await discordBot.StartBot();