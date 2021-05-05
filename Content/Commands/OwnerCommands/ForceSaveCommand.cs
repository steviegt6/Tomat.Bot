using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Tomat.CommandFramework;
using Tomat.CommandFramework.HelpSystem;
using Tomat.Conveniency.Utilities;
using TomatBot.Content.Services;

namespace TomatBot.Content.Commands.OwnerCommands
{
    public class ForceSaveCommand : BaseCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData =>
            new("forcesave", "Forcefully saves all configs on demand as opposed to waiting an entire hour, lmao.");

        public override CommandType CType => CommandType.Hidden;

        [RequireOwnerOrSpecial]
        [Command("forcesave")]
        [Alias("fs")]
        [Summary("Forcefully saves all configs.")]
        public async Task HandleCommand()
        {
            BotStartup.Provider.GetRequiredService<ConfigService>().Config.SaveConfigs();

            Embed embed = EmbedHelper.SuccessEmbed("Saved configuration files!",
                EmbedHelper.CreateSmallEmbed(Context.User).Footer);
            await ReplyAsync(embed: embed);
        }
    }
}