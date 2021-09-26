#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Tomat.Framework.Common.Embeds;
using Tomat.Framework.Core.CommandContext;
using Tomat.Framework.Core.Services.Commands;
using Tomat.Framework.Core.Utilities;

namespace Tomat.TomatBot.Content.Modules.Commands.General
{
    [ModuleInfo("Informative")]
    public sealed class InfoModule : ModuleBase<BotCommandContext>
    {
        #region Help Command

        [Command("help")]
        [Summary("Displays an embed listing all registered commands." +
                 "\nSpecify a command to get a detailed summary of said command.")]
        [Parameters("[command]")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task HelpAsync(string command = "")
        {
            if (command == "")
                await DisplayRegularHelp();
            else
                await DisplayCommandHelp(command);
        }

        public async Task DisplayRegularHelp()
        {
            IEnumerable<EmbedFieldBuilder> modules = Context.Bot.RegisteredCommands.Modules.Select(x =>
                new EmbedFieldBuilder
                {
                    IsInline = true,
                    Name = x.DisplayName,
                    Value = string.Join('\n', x.Commands)
                });

            BaseEmbed embed = new(Context.Bot, Context.User)
            {
                Title = "Command Help",

                Description = "The following is a list of all bot commands." +
                              $"\nAll of these should be prefixed with `{Context.Bot.GetPrefix(Context.Channel)}`.",

                Fields = modules.ToList()
            };

            await ReplyAsync(embed: embed.Build());
        }

        public async Task DisplayCommandHelp(string command)
        {
            command = command.ToLower();

            bool MatchCommand(CommandData match) =>
                match.CommandName == command || match.CommandAliases.Contains(command);

            IEnumerable<CommandData> MatchModule(ModuleData module) =>
                module.Commands.Where(MatchCommand);

            CommandData? data = null;

            foreach (CommandData comData in Context.Bot.RegisteredCommands.Modules.SelectMany(MatchModule))
                data = comData;

            if (!data.HasValue)
            {
                await ReplyAsync(embed: new BaseEmbed(Context.Bot, Context.User)
                {
                    Title = $"Error: no command by the name of `{command}` was found!"
                }.Build());
                return;
            }

            string name = data.Value.CommandName;

            if (!string.IsNullOrEmpty(data.Value.CommandParameters))
                name += $" {data.Value.CommandParameters}";

            if (data.Value.CommandAliases.Length > 0)
                name += $" ({string.Join(", ", data.Value.CommandAliases)})";

            BaseEmbed embed = new(Context.Bot, Context.User)
            {
                Title = name,
                Description = data.Value.CommandDescription
            };

            await ReplyAsync(embed: embed.Build());
        }

        #endregion

        #region Info Command

        // TODO: Invite link.
        [Command("info")]
        [Summary("Displays standard information about the bot namely starter info and bot up-time.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task InfoAsync()
        {
            await ReplyAsync(embed: new BaseEmbed(Context.Bot, Context.User)
            {
                Title = "Basic Bot Info",

                Description =
                    "`Tomat` is a general-purpose Discord bot programmed by `Tomat#9999` with the help of `TheStachelfisch#0395` and other contributors." +
                    $"\n\nFor command help, use `{Context.Bot.GetPrefix(Context.Channel)}help` or ping the bot (`@Tomat help`)." +
                    $"\nBot up-time: `{Context.Bot.UpTime.FormatToString(true)}` (since <t:{Context.Bot.StartTime.ToUnixTimeSeconds()}:F>)"
            }.Build());
        }

        #endregion

        #region Permissions Command

        [Command("permissions")]
        [Alias("perms")]
        [Summary("Lists all current guild and channel permissions that the bot has.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task PermissionsAsync()
        {
            string channelPerms = "";
            string guildPerms;
            SocketGuild? neededGuild = Context.Client.Guilds.FirstOrDefault(x => x == Context.Guild) ?? null;

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

            BaseEmbed embed = new(Context.Bot, Context.User)
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
        public async Task PingAsync()
        {
            int latency = Context.Client.Latency;

            Stopwatch stopwatch = Stopwatch.StartNew();
            IUserMessage? sendAndEdit = await ReplyAsync("Calculating...");
            stopwatch.Stop();

            long responseTime = stopwatch.ElapsedMilliseconds;
            long deltaTime = responseTime - latency;

            BaseEmbed embed = new(Context.Bot, Context.User)
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

        #region Shard Command

        [Command("shard")]
        [Summary("Show shard data for the current guild.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task ShardAsync()
        {
            int shardId = Context.Bot.DiscordClient.GetShardIdFor(Context.Guild);

            await ReplyAsync(embed: new BaseEmbed(Context.Bot, Context.User)
            {
                Title = "Shard Info",
                Description = $"Current guild ({Context.Guild.Id}) is currently using bot shard {shardId}."
            }.Build());
        }

        #endregion
    }
}