using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;

namespace TomatBot.Core.Content.Commands.OwnerCommands
{
    public class UpdateCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("UpdateCommandAsync");

        public override HelpCommandData HelpData => new("update", "Owner-only command, updates the bot from Git.");

        public override CommandType CType => CommandType.Info;

        [RequireOwnerOrSpecial]
        [Command("update")]
        [Summary("Updates the bot.")]
        public async Task UpdateCommandAsync()
        {
            if (!File.Exists("../update.bash"))
            {
                await ReplyAsync($"update.bash file doesn't exist ({MentionUtils.MentionUser(442639987180306432)})");
                return;
            }
            
            IUserMessage? message = await ReplyAsync(embed:new EmbedBuilder
            {
                Title = "Command output",
                Description = "Starting command"
            }.Build());
            
            await File.WriteAllTextAsync("Restarted.txt", $"{Context.Guild.Id} {Context.Channel.Id}");
            
            // If there is something to update the bot will just exit
            string result = "../update.bash".Bash();
            
            await message.ModifyAsync(x => x.Embed = new EmbedBuilder
            {
                Title = "Command output",
                Description = result
            }.Build());
            
            File.Delete("Restarted.txt");
        }
    }
    
    public static class ShellHelper
    {
        public static string Bash(this string cmd)
        {
            string escapedArgs = cmd.Replace("\"", "\\\"");
            
            Process process = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = false
                }
            };

            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
        }
    }
}