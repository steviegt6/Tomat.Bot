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
    public class ForceSaveCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData => new("forcesave", "Forcefully saves all configs on demand as opposed to waiting an entire hour, lmao.");

        public override CommandType CType => CommandType.Hidden;

        [RequireOwnerOrSpecial]
        [Command("forcesave")]
        [Alias("fs")]
        [Summary("Forcefully saves all configs.")]
        public async Task HandleCommand()
        {
            BotStartup.Provider.GetRequiredService<ConfigService>().Config.SaveConfigs();

            Embed embed = EmbedHelper.SuccessEmbed("Saved configuration files!", CreateSmallEmbed().Footer);
            await ReplyAsync(embed: embed);
        }
    }
}