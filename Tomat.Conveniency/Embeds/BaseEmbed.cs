using System;
using Discord;
// ReSharper disable VirtualMemberCallInConstructor

namespace Tomat.Conveniency.Embeds
{
    /// <summary>
    ///     An <see cref="EmbedBuilder" /> with a pre-defined <see cref="EmbedBuilder.Color" />,
    ///     <see cref="EmbedBuilder.Footer" />, <see cref="EmbedBuilder.Author" />, and <see cref="EmbedBuilder.Timestamp" />.
    /// </summary>
    public class BaseEmbed : EmbedBuilder
    {
        // salmon color
        public virtual Color EmbedColor => new(255, 155, 155);

        public virtual string IconURL =>
            "https://cdn.discordapp.com/avatars/800480899300065321/60b55c740acf09b8814777b24e4195f1.png?size=128";

        public virtual string Name => "Tomat";
        
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