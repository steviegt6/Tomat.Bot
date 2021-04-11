using Discord;

namespace TomatBot.Core.Utilities
{
    public class EmbedHelper
    {
        /// <summary>
        ///     Creates a green embed with the title "Success!".
        /// </summary>
        /// <param name="description">The embed's description.</param>
        /// <param name="footer">The embed's footer.</param>
        /// <returns>A <c>built</c> <see cref="Embed"/>.</returns>
        public static Embed SuccessEmbed(string description, EmbedFooterBuilder footer = null!)
        {
            EmbedBuilder successEmbed = new()
            {
                Title = "Success!",
                Color = Color.Green,
                Description = description
            };
            successEmbed.WithCurrentTimestamp();

            if (footer != null!)
                successEmbed.WithFooter(footer);

            return successEmbed.Build();
        }

        /// <summary>
        ///     Creates a red embed with the title "Error!".
        /// </summary>
        /// <param name="description">The embed's description.</param>
        /// <param name="footer">The embed's footer.</param>
        /// <returns>A <c>built</c> <see cref="Embed"/>.</returns>
        public static Embed ErrorEmbed(string description, EmbedFooterBuilder footer = null!)
        {
            EmbedBuilder successEmbed = new()
            {
                Title = "Error!",
                Color = Color.Red,
                Description = description
            };
            successEmbed.WithCurrentTimestamp();

            if (footer != null!)
                successEmbed.WithFooter(footer);
            
            return successEmbed.Build();
        }
    }
}