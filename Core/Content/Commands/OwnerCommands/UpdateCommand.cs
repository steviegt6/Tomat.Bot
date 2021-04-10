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
        //TODO: Change this Stevie, I don't know how your Framework works
        public override MethodInfo? AssociatedMethod { get; }
        public override HelpCommandData HelpData { get; }
        public override CommandType CType { get; }

        [Command("update")]
        [RequireOwnerOrSpecial]
        [Summary("Updates the bot")]
        public async Task UpdateCommandAsync()
        {
            if (!File.Exists("../update.bash"))
            {
                await ReplyAsync($"update.bash file doesn't exist ({MentionUtils.MentionUser(442639987180306432)})");
                return;
            }
            
            var message = await ReplyAsync(embed:new EmbedBuilder
            {
                Title = "Command output",
                Description = "Starting command... (If there is something to update then the bot will be offline now)"
            }.Build());
            
            // If there is something to update the bot will just exit
            string result = "../update.bash".Bash();
            
            await message.ModifyAsync(x => x.Embed = new EmbedBuilder
            {
                Title = "Command output",
                Description = result
            }.Build());
        }
    }
    
    public static class ShellHelper
    {
        public static string Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            
            var process = new Process
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