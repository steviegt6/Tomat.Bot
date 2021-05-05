using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Tomat.CommandFramework;
using Tomat.CommandFramework.HelpSystem;
using Tomat.Conveniency.Utilities;
using Tomat.TomatBot.Content.Services;

namespace Tomat.TomatBot.Content.Commands.OwnerCommands
{
    public class ForceLoadCommand : BaseCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData => new("forceload", "Forcefully loads all configs on demand");

        public override CommandType CType => CommandType.Hidden;

        [RequireOwnerOrSpecial]
        [Command("forceload")]
        [Alias("fl")]
        [Summary("Forcefully loads all configs.")]
        public async Task HandleCommand()
        {
            await BotStartup.Provider.GetRequiredService<ConfigService>().Config.LoadConfigs();

            await ReplyAsync(embed: EmbedHelper.SuccessEmbed("Loaded configuration files!",
                EmbedHelper.CreateSmallEmbed(Context.User).Footer));
        }
    }
}