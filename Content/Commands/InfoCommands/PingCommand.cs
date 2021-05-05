using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Tomat.CommandFramework;
using Tomat.CommandFramework.HelpSystem;
using Tomat.Conveniency.Embeds;

namespace TomatBot.Content.Commands.InfoCommands
{
    public sealed class PingCommand : BaseCommand
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
            int latency = Context.Client.Latency;

            Stopwatch stopwatch = Stopwatch.StartNew();
            IUserMessage? sendAndEdit = await ReplyAsync("Calculating...");
            stopwatch.Stop();

            long responseTime = stopwatch.ElapsedMilliseconds;
            long deltaTime = responseTime - latency;

            BaseEmbed embed = new(Context.User)
            {
                Title = "Ping",

                Description = $"Latency - `{latency}ms`" +
                              $"\nResponse Time - `{responseTime}ms`" +
                              $"\nDelta Time - `{deltaTime}ms`"
            };

            await sendAndEdit.ModifyAsync(x =>
            {
                x.Content = "";
                x.Embed = embed.Build();
            });
        }
    }
}