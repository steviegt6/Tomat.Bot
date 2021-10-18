#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Tomat.Framework.Common.Embeds;
using Tomat.Framework.Core.CommandContext;
using Tomat.Framework.Core.Services.Commands;
using Victoria;

namespace Tomat.TomatBot.Content.Modules.Commands.Audio
{
    [ModuleInfo("Audio/Music")]
    public sealed class AudioModule : ModuleBase<BotCommandContext>
    {
        [Command("join")]
        [Alias("joinvc")]
        public async Task JoinVoiceChannelAsync()
        {
            LavaNode node = Context.Bot.ServiceProvider.GetRequiredService<LavaNode>();

            if (node.HasPlayer(Context.Guild))
            {
                await ReplyAsync(embed: EmbedHelper.ErrorEmbed("Already connected to a voice channel in this guild!"));
                return;
            }

            if (Context.User is not IVoiceState voiceState)
            {
                await ReplyAsync(embed: EmbedHelper.ErrorEmbed("Could not find a voice state for your user!"));
                return;
            }

            if (voiceState.VoiceChannel is null)
            {
                await ReplyAsync(embed: EmbedHelper.ErrorEmbed("You are not in a voice channel in this guild!"));
                return;
            }

            try
            {
                if (!node.IsConnected)
                    await node.ConnectAsync();

                await node.JoinAsync(voiceState.VoiceChannel, Context.Channel as ITextChannel);
                await ReplyAsync(embed: EmbedHelper.SuccessEmbed($"Joined channel: `{voiceState.VoiceChannel.Name}`!"));
            }
            catch (Exception e)
            {
                await ReplyAsync(embed: new BaseEmbed(Context.Bot, Context.User, Color.Red)
                {
                    Title = "Exception thrown: " + e.GetType().Name,

                    Description = e.ToString()
                }.Build());
            }
        }
    }
}