using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Tomat.CommandFramework;
using Tomat.CommandFramework.HelpSystem;
using Tomat.TomatBot.Utilities;

namespace Tomat.TomatBot.Content.Commands.OwnerCommands
{
    public class UpdateCommand : BaseCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("UpdateCommandAsync");

        public override HelpCommandData HelpData => new("update", "Owner-only command, updates the bot from Git.");

        public override CommandType CType => CommandType.Hidden;

        [RequireOwnerOrSpecial]
        [Command("update")]
        [Summary("Updates the bot.")]
        public async Task UpdateCommandAsync()
        {
            if (!File.Exists("../TomatUpdate.bash"))
            {
                await ReplyAsync(
                    $"TomatUpdate.bash file doesn't exist ({MentionUtils.MentionUser(442639987180306432)})");
                return;
            }

            IUserMessage? message = await ReplyAsync(embed: new EmbedBuilder
            {
                Title = "Command output",
                Description = "Starting command"
            }.Build());

            await File.WriteAllTextAsync("Restarted.txt", $"{Context.Guild.Id} {Context.Channel.Id}");

            // If there is something to update the bot will just exit
            string result = "../TomatUpdate.bash".Bash();

            await message.ModifyAsync(x => x.Embed = new EmbedBuilder
            {
                Title = "Command output",
                Description = result
            }.Build());

            File.Delete("Restarted.txt");
        }
    }
}