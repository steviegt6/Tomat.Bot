using Discord;

namespace TomatBot.Core.Utilities
{
    public class EmbedHelper
    {
        public static Embed SuccessEmbed(string description, EmbedFooterBuilder footer = null)
        {
            var successEmbed = new EmbedBuilder();

            successEmbed.WithTitle("Success!");
            successEmbed.WithColor(Color.Green);
            successEmbed.WithDescription(description);
            if (footer != null)
                successEmbed.WithFooter(footer);
            successEmbed.WithCurrentTimestamp();
            
            return successEmbed.Build();
        }

        public static Embed ErrorEmbed(string description, EmbedFooterBuilder footer = null)
        {
            var successEmbed = new EmbedBuilder();

            successEmbed.WithTitle("Error!");
            successEmbed.WithColor(Color.Red);
            successEmbed.WithDescription(description);
            if (footer != null)
                successEmbed.WithFooter(footer);
            successEmbed.WithCurrentTimestamp();
            
            return successEmbed.Build();
        }
    }
}