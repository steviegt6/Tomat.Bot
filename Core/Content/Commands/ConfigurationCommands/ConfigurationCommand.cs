using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using TomatBot.Core.Content.Configs;
using TomatBot.Core.Content.Services;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;
using TomatBot.Core.Utilities;

namespace TomatBot.Core.Content.Commands.ConfigurationCommands
{
    public sealed class ConfigurationCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData => new("configure", $"Allows you to configure various aspects about the bot. Do `{BotStartup.DefaultPrefix}configure help` for more.");

        public override CommandType CType => CommandType.Configuration;

        public override string Parameters => "<option> [value]";

        [Command("configure")]
        [Alias("config, cfg")]
        [Summary("Provides a way to configure the bot.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task HandleCommand(string option = "", string value = "")
        {
            if (string.IsNullOrEmpty(option))
            {
                await ReplyAsync(embed: CreateSmallEmbed($"Do \"`{BotStartup.DefaultPrefix}configure help`\" for information on bot configuration!").Build());
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
                Embed embed = EmbedHelper.ErrorEmbed("Somehow unable to locate config for your guild! Please contact the developers.",
                    CreateSmallEmbed().Footer);
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
                                CreateSmallEmbed().Footer);
                            await ReplyAsync(embed: embed);
                        }
                        else
                        {
                            guildConfig.guildPrefix = value;
                            Embed embed = EmbedHelper.SuccessEmbed(
                                $"Changed bot prefix to `{value}`",
                                CreateSmallEmbed().Footer);
                            await ReplyAsync(embed: embed);
                        }
                    break;
            }
        }
    }
}
