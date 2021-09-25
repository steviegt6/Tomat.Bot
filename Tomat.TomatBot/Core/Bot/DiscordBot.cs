#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Tomat.TomatBot.Core.Configuration;
using Tomat.TomatBot.Core.Services;
using Tomat.TomatBot.Core.Services.Commands;
using Tomat.TomatBot.Core.Utilities;
using IntervalTimer = System.Timers.Timer;

namespace Tomat.TomatBot.Core.Bot
{
    public abstract class DiscordBot : IDisposable, IServicer, IConfigurable
    {
        #region Properties

        public virtual CancellationTokenSource StopTokenSource { get; }

        public virtual CancellationToken StopToken => StopTokenSource.Token;

        public virtual IServiceCollection Services { get; }

        public virtual ServiceProvider ServiceProvider => Services.BuildServiceProvider();

        public virtual TimeSpan UpTime => DateTimeOffset.Now - StartTime;

        public virtual DateTimeOffset StartTime { get; }

        public virtual DiscordSocketClient DiscordClient => ServiceProvider.GetRequiredService<DiscordSocketClient>();

        public virtual IEnumerable<Assembly> Assemblies
        {
            get { yield return GetType().Assembly; }
        }

        public virtual IUser User => DiscordClient.CurrentUser;

        public string Token { get; }

        public CommandRepository RegisteredCommands { get; protected set; }

        #endregion

        #region Constructor and Desconstructor

        protected DiscordBot(string token)
        {
            Token = token;

            StopTokenSource = new CancellationTokenSource();
            ServiceCollection services = new();

            AppDomain.CurrentDomain.ProcessExit += (_, _) => Dispose();

            StartTime = DateTime.Now;

            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                ExclusiveBulkDelete = true,
                DefaultRetryMode = RetryMode.RetryRatelimit,
                AlwaysDownloadUsers = true,
                ConnectionTimeout = 30 * 1000,
                MessageCacheSize = 50
            }));

            Services = services;
            RegisteredCommands = new CommandRepository();
        }

        ~DiscordBot()
        {
            Dispose(false);
        }

        #endregion

        #region Methods

        public async Task StartBot()
        {
            // RegisteredCommands = new CommandRepository();
            await RegisteredCommands.RegisterFromBot(this);

            await SetupSingletons();

            DiscordClient.Log += LogMessageAsync;
            await DiscordClient.LoginAsync(TokenType.Bot, Token);

            await DiscordClient.StartAsync();

            // Initialize *after* starting the bot.
            await ServiceRegistry.InitializeServicesAsync(this);

            DiscordClient.Ready += async () =>
            {
                await RestartUtilities.CheckForRestart(this);

                new IntervalTimer(60 * 60 * 1000)
                {
                    AutoReset = true,
                    Enabled = true
                    // ReSharper disable once AsyncVoidLambda
                }.Elapsed += async (_, _) => await SaveConfig();

                await LoadConfig();

                await Task.CompletedTask;
            };

            await OnStartAsync();

            try
            {
                await Task.Delay(-1, StopToken);
            }
            catch (TaskCanceledException)
            {
            }
        }

        public virtual async Task OnStartAsync()
        {
            await Task.CompletedTask;
        }

        public virtual async Task LogMessageAsync(LogMessage arg)
        {
            if (arg.Severity == LogSeverity.Verbose && !Debugger.IsAttached)
            {
                await Task.CompletedTask;
                return;
            }

            static string Date(DateTime time)
            {
                StringBuilder builder = new();

                if (time.Hour < 10)
                    builder.Append('0');

                builder.Append(time.Hour);
                builder.Append(':');

                if (time.Minute < 10)
                    builder.Append('0');

                builder.Append(time.Minute);
                builder.Append(':');

                if (time.Second < 10)
                    builder.Append('0');

                builder.Append(time.Second);

                return builder.ToString();
            }

            static string Padded(string text)
            {
                int padding = 8 - text.Length;
                StringBuilder builder = new();

                builder.Append('[');
                builder.Append(' ', padding);
                builder.Append(text);
                builder.Append(']');

                return builder.ToString();
            }

            DateTime now = DateTime.Now;
            DateTime utc = DateTime.UtcNow;

            string severity = Padded(arg.Severity.ToString());
            string source = Padded(arg.Source ?? "N/A");

            string message = $"{severity} [{Date(now)}/(UTC) {Date(utc)}] {source}: {arg.Message}";

            Console.WriteLine(message);
            
            if (arg.Exception is not null)
                Console.WriteLine(arg.Exception);

            await Task.CompletedTask;
        }

        public virtual async Task SetupSingletons()
        {
            Services.AddSingleton(new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                IgnoreExtraArgs = true,
                DefaultRunMode = RunMode.Async
            })).AddSingleton(new CommandReceiver(ServiceProvider, DiscordClient, this));

            await Task.CompletedTask;
        }

        public virtual async Task LoadConfig() => await Task.CompletedTask;

        public virtual async Task SaveConfig() => await Task.CompletedTask;

        public abstract string GetPrefix(ISocketMessageChannel channel);

        #endregion

        #region Disposing

        protected virtual async void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            await SaveConfig();

            StopTokenSource.Cancel();
            await ServiceProvider.DisposeAsync();
            await DiscordClient.StopAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}