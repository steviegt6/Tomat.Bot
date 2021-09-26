#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using Discord;
using Tomat.TomatBot.Common.Embeds;
using Tomat.TomatBot.Core.Bot;

namespace TomatBot.Framework.Common.Embeds
{
    public static class EmbedHelper
    {
        /// <summary>
        ///     Creates a green embed with the title "Success!".
        /// </summary>
        /// <param name="description">The embed's description.</param>
        /// <param name="footer">The embed's footer.</param>
        /// <returns>A <c>built</c> <see cref="Embed"/>.</returns>
        public static Embed SuccessEmbed(string description, EmbedFooterBuilder? footer = null)
        {
            EmbedBuilder successEmbed = new()
            {
                Title = "Success!",
                Color = Color.Green,
                Description = description
            };

            successEmbed.WithCurrentTimestamp();

            if (footer != null)
                successEmbed.WithFooter(footer);

            return successEmbed.Build();
        }

        /// <summary>
        ///     Creates a red embed with the title "Error!".
        /// </summary>
        /// <param name="description">The embed's description.</param>
        /// <param name="footer">The embed's footer.</param>
        /// <returns>A <c>built</c> <see cref="Embed"/>.</returns>
        public static Embed ErrorEmbed(string description, EmbedFooterBuilder? footer = null)
        {
            EmbedBuilder successEmbed = new()
            {
                Title = "Error!",
                Color = Color.Red,
                Description = description
            };

            successEmbed.WithCurrentTimestamp();

            if (footer != null)
                successEmbed.WithFooter(footer);

            return successEmbed.Build();
        }

        public static BaseEmbed CreateSmallEmbed(this DiscordBot bot, IUser user, string text = "",
            Color? embedColor = null) => new(bot, user, embedColor)
        {
            Description = string.IsNullOrEmpty(text)
                ? "I'm a bot, and this action was performed automatically. If you have a problem with that, go back to Reddit."
                : text
        };
    }
}