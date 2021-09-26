﻿#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Discord.WebSocket;
using Tomat.Framework.Core.Bot;
using Tomat.TatsuSharp;
using Tomat.TomatBot.Content.Activities;
using Tomat.TomatBot.Content.Configuration;
using Tomat.TomatBot.Core.Tatsu;

namespace Tomat.TomatBot.Content
{
    public class TomatBot : DiscordBot, ITatsuProvider
    {
        public TatsuClient TatsuClient { get; }

        public GlobalBotConfig GlobalConfig { get; }

        public string TatsuToken => TatsuClient.APIKey;

        public TomatBot(string token, string tatsuToken) : base(token)
        {
            TatsuClient = new TatsuClient(tatsuToken);
            GlobalConfig = new GlobalBotConfig(this);
        }

        public override async Task OnStartAsync()
        {
            DiscordClient.JoinedGuild += async _ =>
            {
                // Run every time the bot joins a guild to update the current count.
                await DiscordClient.SetActivityAsync(new StatisticsActivity(DiscordClient.Guilds));
            };

            DiscordClient.ShardReady += async _ =>
            {
                // Refresh when a new shard is readied.
                await DiscordClient.SetActivityAsync(new StatisticsActivity(DiscordClient.Guilds));
            };

            await DiscordClient.SetStatusAsync(UserStatus.DoNotDisturb);
        }
        
        public override string GetPrefix(ISocketMessageChannel channel)
        {
            string prefix = Debugger.IsAttached ? "edge!" : "tomat!";

            if (channel is not SocketGuildChannel gChannel)
                return prefix;

            // Gotta be careful to make sure this isn't null or whitespace or empty.
            string gPrefix = GlobalConfig.GetGuildConfig(gChannel.Guild.Id).GuildPrefix;

            return string.IsNullOrEmpty(gPrefix) ? prefix : gPrefix;
        }

        public override async Task SaveConfig() => await GlobalConfig.SaveConfig();

        public override async Task LoadConfig() => await GlobalConfig.LoadConfig();

        protected override async void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing)
                return;

            TatsuClient.Client.Dispose();
            await TatsuClient.Bucket.Refill();
        }
    }
}