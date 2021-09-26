#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System.Diagnostics;
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
            DiscordClient.Ready += async () =>
            {
                await DiscordClient.SetActivityAsync(new StatisticsActivity(DiscordClient.Guilds));

                new Timer(10 * 1000)
                {
                    AutoReset = true,
                    Enabled = true
                    // ReSharper disable once AsyncVoidLambda
                }.Elapsed += async (_, _) =>
                    await DiscordClient.SetActivityAsync(new StatisticsActivity(DiscordClient.Guilds));

                await Task.CompletedTask;
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