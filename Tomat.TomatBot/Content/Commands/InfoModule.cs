using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Tomat.Conveniency.Embeds;
using Tomat.Conveniency.Utilities;
using Tomat.TomatBot.Common;

namespace Tomat.TomatBot.Content.Commands
{
    [ModuleInfo("Informative")]
    public sealed class InfoModule : ModuleBase<SocketCommandContext>
    {
        #region Help Command

        [Command("help")]
        [Summary(
            "Displays an embed containing basic information on all registered commands. Specify a command for a detailed summary.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [Parameters("[command]")]
        public async Task HelpAsync(string command = "")
        {
            if (command == "")
                await DisplayRegularHelp();
            else
                await DisplayCommandHelp(command);
        }

        private async Task DisplayRegularHelp()
        {
            List<EmbedFieldBuilder> embedFields = CommandHandler.Registry.Modules.Select(data =>
                new EmbedFieldBuilder
                    {IsInline = true, Name = data.displayName, Value = string.Join('\n', data.commands)}).ToList();

            BaseEmbed embed = new(Context.User)
            {
                Title = "Command Help",

                Description = "The following is a list of all bot commands." +
                              $"\nAll of these should be prefixed with `{BotStartup.GetGuildPrefix(Context.Guild)}`. " +
                              $"\n(If your guild has changed the prefix, you can also use `{BotStartup.DefaultPrefix}`)",
                Fields = embedFields
            };

            await ReplyAsync(embed: embed.Build());
        }

        public async Task DisplayCommandHelp(string command)
        {
            command = command.ToLower();

            CommandHandler.Registry.ModuleData.CommandData? data = null;

            foreach (CommandHandler.Registry.ModuleData.CommandData commandData in
                CommandHandler.Registry.Modules.SelectMany(module => module.commands.Where(commandData =>
                    commandData.commandName == command || commandData.commandAliases.Contains(command))))
            {
                data = commandData;
                goto FullBreak;
            }

            if (!data.HasValue)
            {
                await ReplyAsync(embed: new BaseEmbed(Context.User)
                {
                    Title = $"Error: no command by the name of `{command}` was found!"
                }.Build());
                return;
            }

            FullBreak:

            string name = $"{data.Value.commandName}";

            if (data.Value.commandParameters != "")
                name += $" {data.Value.commandParameters}";

            if (data.Value.commandAliases != Array.Empty<string>())
                name += $" ({string.Join(", ", data.Value.commandAliases)})";

            string description = data.Value.commandDescription;

            BaseEmbed embed = new(Context.User)
            {
                Title = name,
                Description = description
            };

            await ReplyAsync(embed: embed.Build());
        }

        #endregion

        #region Info Command

        [Command("info")]
        [Summary(
            "Displays standard information about the bot, including developer contact information and a bot invitation link.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task InfoAsync()
        {
            BaseEmbed embed = new(Context.User)
            {
                Title = "Basic Bot Info",

                Description =
                    "`Tomat` is a general-purpose Discord bot programmed by `convicted tomatophile#0001` with the help of `TheStachelfisch#0395`, who also hosts." +
                    $"\n\nFor command help, use `{BotStartup.DefaultPrefix}help` or ping the bot directly (`@Tomat help`)" +
                    $"\nBot up-time: `{BotStartup.UpTime.FormatToString(true)}`" +
                    "\n\n\nClick [here](https://discord.com/api/oauth2/authorize?client_id=800480899300065321&permissions=93248&scope=bot) to invite Tomat to your server."
            };

            await ReplyAsync(embed: embed.Build());
        }

        #endregion

        #region Permissions Command

        [Command("permissions")]
        [Alias("perms")]
        [Summary("Lists all current guild and channel permissions the bot has.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task PermissionsAsync()
        {
            string channelPerms = "";
            string guildPerms;
            SocketGuild? neededGuild = BotStartup.Client.Guilds.FirstOrDefault(x => x == Context.Guild) ?? null;

            if (neededGuild != null)
            {
                guildPerms = neededGuild.CurrentUser.GuildPermissions.ToList()
                    .Aggregate("", (current, gPerm) => current + $"\n * {gPerm}");

                if (Context.Channel is IGuildChannel guildChannel)
                    channelPerms = neededGuild.CurrentUser.GetPermissions(guildChannel).ToList()
                        .Aggregate("", (current, cPerm) => current + $"\n * {cPerm}");
                else
                    channelPerms = "unable to fetch current channel";
            }
            else
                guildPerms = "unable to fetch guild";

            BaseEmbed embed = new(Context.User)
            {
                Title = "Permissions",

                Fields = new List<EmbedFieldBuilder>
                {
                    new()
                    {
                        IsInline = true,
                        Name = "Guild Permissions",
                        Value = guildPerms
                    },

                    new()
                    {
                        IsInline = true,
                        Name = "Channel Permissions",
                        Value = channelPerms
                    }
                }
            };

            await ReplyAsync(embed: embed.Build());
        }

        #endregion

        #region Ping Command

        [Command("ping")]
        [Alias("pong")]
        [Summary("Shows bot gateway latency and response time.")]
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

        #endregion
    }
}