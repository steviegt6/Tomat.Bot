using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Tomat.Conveniency.Utilities;
using Tomat.TomatBot.Common;
using Tomat.TomatBot.Content.Configs;
using Tomat.TomatBot.Content.Services;

namespace Tomat.TomatBot.Content.Commands
{
    [ModuleInfo("Configuration")]
    public sealed class ConfigurationModule : ModuleBase<SocketCommandContext>
    {
        [Command("configure")]
        [Alias("config, cfg")]
        [Summary("Allows the configuration of various aspects of Tomat. Run this command with `help` as the only parameter for more.")]
        [Parameters("<option> [value]")]
        public async Task ConfigureAsync(string option = "", string value = "")
        {
            if (string.IsNullOrEmpty(option))
            {
                await ReplyAsync(embed: EmbedHelper.CreateSmallEmbed(Context.User,
                        $"Do \"`{BotStartup.DefaultPrefix}configure help`\" for information on bot configuration!")
                    .Build());
                return;
            }

            await ParseConfigCommand(option, value);
        }

        private static bool VerifyValueParameter(string value) => !string.IsNullOrEmpty(value);

        private async Task ParseConfigCommand(string option = "", string value = "")
        {
            BotConfig config = BotStartup.Provider.GetRequiredService<ConfigService>().Config;
            GuildConfig? guildConfig = config.Guilds.FirstOrDefault(x => x.associatedId == Context.Guild.Id);

            if (guildConfig == null)
            {
                Embed embed = EmbedHelper.ErrorEmbed(
                    "Somehow unable to locate config for your guild! Please contact the developers.",
                    EmbedHelper.CreateSmallEmbed(Context.User).Footer);
                await ReplyAsync(embed: embed);
                return;
            }

            // TODO: "help" display

            switch (option.ToLower())
            {
                case "changeprefix":
                    if (VerifyValueParameter(value))
                        if (!(Context.User as SocketGuildUser)!.GuildPermissions.Administrator)
                        {
                            Embed embed = EmbedHelper.ErrorEmbed(
                                "You must be an administrator to modify the bot's prefix!",
                                EmbedHelper.CreateSmallEmbed(Context.User).Footer);
                            await ReplyAsync(embed: embed);
                        }
                        else
                        {
                            guildConfig.guildPrefix = value;
                            Embed embed = EmbedHelper.SuccessEmbed(
                                $"Changed bot prefix to `{value}`",
                                EmbedHelper.CreateSmallEmbed(Context.User).Footer);
                            await ReplyAsync(embed: embed);
                        }

                    break;
            }
        }
    }
}