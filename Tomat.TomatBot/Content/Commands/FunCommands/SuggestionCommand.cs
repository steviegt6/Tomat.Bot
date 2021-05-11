using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Tomat.CommandFramework;
using Tomat.CommandFramework.HelpSystem;
using Tomat.Conveniency.Embeds;

namespace Tomat.TomatBot.Content.Commands.FunCommands
{
    public sealed class SuggestionCommand : BaseCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData =>
            new("suggestion", "Creates a suggestion embed with an +1, -1, and indifferent option.");

        public override CommandType CType => CommandType.Fun;

        [Command("suggestion")]
        [Alias("suggest")]
        [Summary("Creates a suggestion embed.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [RequireBotPermission(ChannelPermission.AddReactions)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task HandleCommand([Remainder] string suggestion)
        {
            await Context.Message.DeleteAsync();

            EmbedBuilder realEmbed = new BaseEmbed(Context.User)
            {
                Title = "Suggestion!",
                Color = Color.Gold,
                Description = $"{suggestion}" +
                              $"\n\n" +
                              $"👍: +1" +
                              $"👎: -1" +
                              $"🖕 (lol): Indifferent"
            };

            IUserMessage? message = await ReplyAsync(embed: realEmbed.Build());
            await message.AddReactionsAsync(new IEmote[] {new Emoji("👍"), new Emoji("👎"), new Emoji("🖕") });
        }
    }
}