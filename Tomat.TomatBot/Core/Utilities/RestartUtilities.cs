#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Tomat.TomatBot.Common.Logging;
using Tomat.TomatBot.Core.Bot;

namespace Tomat.TomatBot.Core.Utilities
{
    public static class RestartUtilities
    {
        public const string RestartFile = "Restarted.txt";

        public static async Task CheckForRestart(DiscordBot bot)
        {
            LogMessage log = new LogBuilder().WithMessage("Verifying restart...").WithSource("Restart").Build();

            await bot.LogMessageAsync(log);

            if (File.Exists(RestartFile))
            {
                try
                {
                    LogMessage found = new LogBuilder().WithMessage($"Found {RestartFile} file!").WithSource("Restart")
                        .Build();

                    await bot.LogMessageAsync(found);

                    string[] ids = File.ReadAllTextAsync(RestartFile).Result.Split(' ');

                    if (ids.Length < 2)
                        return;

                    Embed embed = new EmbedBuilder
                    {
                        Title = "Bot restarted successfully!",
                        Color = Color.Green
                    }.Build();

                    ulong guildId = ulong.Parse(ids[0]);
                    ulong channelId = ulong.Parse(ids[1]);
                    SocketTextChannel channel = (SocketTextChannel) bot.DiscordClient
                        .GetGuild(guildId).GetChannel(channelId);

                    await channel.SendMessageAsync(embed: embed);
                }
                catch (Exception e)
                {
                    LogMessage exception = new LogBuilder().WithMessage("Caught restart exception:")
                        .WithSource("Restart").WithException(e).Build();

                    await bot.LogMessageAsync(exception);
                }
                finally
                {
                    File.Delete(RestartFile);
                }
            }
        }
    }
}