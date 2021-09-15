#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using Discord;

namespace Tomat.TomatBot.Common.Embeds
{
    public class BaseEmbed : EmbedBuilder
    {
        public const string Profile =
            "https://cdn.discordapp.com/avatars/800480899300065321/60b55c740acf09b8814777b24e4195f1.png?size=128";

        // salmon color
        public Color EmbedColor => new(255, 155, 155);

        public string IconURL => Profile;

        public string Name => "Tomat";

        public BaseEmbed(IUser user)
        {
            Color = EmbedColor;

            Footer = new EmbedFooterBuilder
            {
                Text = $"Requested by {user.Username}",
                IconUrl = user.GetAvatarUrl()
            };

            Author = new EmbedAuthorBuilder
            {
                IconUrl = IconURL,
                Name = Name
            };

            Timestamp = DateTime.UtcNow;
        }
    }
}