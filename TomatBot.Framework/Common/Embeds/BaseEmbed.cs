#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using Discord;
using Tomat.TomatBot.Core.Bot;

namespace Tomat.TomatBot.Common.Embeds
{
    public class BaseEmbed : EmbedBuilder
    {
        public Color EmbedColor { get; }

        public BaseEmbed(DiscordBot bot, IUser user, Color? embedColor = null) : this(bot.User, user, embedColor)
        {
        }

        public BaseEmbed(IUser bot, IUser user, Color? embedColor = null)
        {
            // salmon color
            Color = EmbedColor = embedColor ?? new Color(255, 155, 155);

            Footer = new EmbedFooterBuilder
            {
                Text = $"Requested by {user.Username}",
                IconUrl = user.GetAvatarUrl()
            };

            Author = new EmbedAuthorBuilder
            {
                IconUrl = bot.GetAvatarUrl(),
                Name = bot.Username
            };

            Timestamp = DateTime.UtcNow;
        }
    }
}