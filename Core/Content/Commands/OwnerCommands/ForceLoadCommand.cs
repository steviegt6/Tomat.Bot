using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using TomatBot.Core.Content.Services;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;
using TomatBot.Core.Utilities;

namespace TomatBot.Core.Content.Commands.OwnerCommands
{
    public class ForceLoadCommand : TomatCommand
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
            
            await ReplyAsync(embed: EmbedHelper.SuccessEmbed("Loaded configuration files!", CreateSmallEmbed().Footer));
        }
    }
}