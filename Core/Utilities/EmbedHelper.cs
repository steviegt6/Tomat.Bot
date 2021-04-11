using Discord;

namespace TomatBot.Core.Utilities
{
    public class EmbedHelper
    {
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