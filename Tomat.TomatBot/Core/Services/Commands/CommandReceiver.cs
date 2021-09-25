#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Tomat.TomatBot.Common.Embeds;
using Tomat.TomatBot.Core.Bot;
using Tomat.TomatBot.Core.Utilities;

namespace Tomat.TomatBot.Core.Services.Commands
{
    public class CommandReceiver : IInitializableService
    {
        public IServiceProvider ServiceProvider { get; }

        public DiscordSocketClient Client { get; }

        public DiscordBot Bot { get; }

        public CommandService? Commands { get; protected set; }

        public CommandReceiver(IServiceProvider serviceProvider, DiscordSocketClient client, DiscordBot bot)
        {
            ServiceProvider = serviceProvider;
            Client = client;
            Bot = bot;
        }

        public async Task InitializeAsync()
        {
            Commands = ServiceProvider.GetRequiredService<CommandService>();

            Client.MessageReceived += ReceiveCommand;
            Commands.CommandExecuted += HandleErrors;

            foreach (Assembly assembly in Bot.Assemblies)
                await Commands.AddModulesAsync(assembly, ServiceProvider);

            await Task.CompletedTask;
        }

        private async Task ReceiveCommand(SocketMessage message)
        {
            if (message.ValidateMessageMention(Bot, out CommandUtilities.InvalidMessageReason invalidReason, out int argPos, Client))
                await Commands!.ExecuteAsync(new SocketCommandContext(Client, message as SocketUserMessage),
                    argPos, null);
        }

        private async Task HandleErrors(Optional<CommandInfo> info, ICommandContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                BaseEmbed embed = new(Client.CurrentUser, context.User)
                {
                    Title = $"Error encountered: {result.Error}",
                    Description = result.ErrorReason
                };

                await context.Channel.SendMessageAsync(embed: embed.Build());
            }
        }
    }
}