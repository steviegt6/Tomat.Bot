#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Discord.WebSocket;
using Tomat.TatsuSharp;
using Tomat.TomatBot.Content.Activities;
using Tomat.TomatBot.Core.Bot;
using Tomat.TomatBot.Core.Tatsu;

namespace Tomat.TomatBot.Content
{
    public class TomatBot : DiscordBot, ITatsuProvider
    {
        public TatsuClient TatsuClient { get; }

        public string TatsuToken => TatsuClient.APIKey;

        public TomatBot(string token, string tatsuToken) : base(token)
        {
            TatsuClient = new TatsuClient(tatsuToken);
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
                }.Elapsed += async (_, _) =>
                    await DiscordClient.SetActivityAsync(new StatisticsActivity(DiscordClient.Guilds));

                await Task.CompletedTask;
            };

            await DiscordClient.SetStatusAsync(UserStatus.DoNotDisturb);
        }

        // TODO: Config!
        public override string GetPrefix(ISocketMessageChannel channel) => Debugger.IsAttached ? "edge!" : "tomat!";

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