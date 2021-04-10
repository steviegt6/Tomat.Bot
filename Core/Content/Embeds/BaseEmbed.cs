using System;
using Discord;

namespace TomatBot.Core.Content.Embeds
{
    /// <summary>
    ///     An <see cref="EmbedBuilder" /> with a pre-defined <see cref="EmbedBuilder.Color" />,
    ///     <see cref="EmbedBuilder.Footer" />, <see cref="EmbedBuilder.Author" />, and <see cref="EmbedBuilder.Timestamp" />.
    /// </summary>
    public class BaseEmbed : EmbedBuilder
    {
        public BaseEmbed(IUser user)
        {
            Color = new Color(255, 155, 155); // salmon color

            Footer = new EmbedFooterBuilder
            {
                Text = $"Requested by {user.Username}",
                IconUrl = user.GetAvatarUrl()
            };

            Author = new EmbedAuthorBuilder
            {
                IconUrl =
                    "https://cdn.discordapp.com/avatars/800480899300065321/60b55c740acf09b8814777b24e4195f1.png?size=128",
                Name = "Tomat"
            };

            Timestamp = DateTime.UtcNow;
        }
    }
}