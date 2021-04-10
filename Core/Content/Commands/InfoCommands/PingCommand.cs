using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TomatBot.Core.Content.Embeds;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;

namespace TomatBot.Core.Content.Commands.InfoCommands
{
    public sealed class PingCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData => new("ping", "Shows bot gateway latency and response time.");

        public override CommandType CType => CommandType.Info;

        [Command("ping")]
        [Alias("pong")] // we do a little bit of trolling
        [Summary("Shows bot latency.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task HandleCommand()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            IUserMessage? toDelete = await ReplyAsync("Calculating...");
            stopwatch.Stop();

            await toDelete.DeleteAsync();

            BaseEmbed embed = new(Context.User)
            {
                Title = "Ping",

                Description = $"Latency - `{Context.Client.Latency}ms`" +
                              $"\nResponse Time - `{stopwatch.ElapsedMilliseconds}ms`"
            };

            await ReplyAsync(embed: embed.Build());
        }
    }
}
