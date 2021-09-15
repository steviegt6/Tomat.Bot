#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System.IO;
using Tomat.TomatBot.Content;
using Tomat.TomatBot.Core.Bot;

string token = await File.ReadAllTextAsync("token.txt");
string tatsu = await File.ReadAllTextAsync("tatsu.txt");

using DiscordBot discordBot = new TomatBot(token, tatsu);
await discordBot.StartBot();