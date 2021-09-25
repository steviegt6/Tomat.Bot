﻿#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Tomat.TomatBot.Common.Embeds;
using Tomat.TomatBot.Core.CommandContext;
using Tomat.TomatBot.Core.Services.Commands;
using Tomat.TomatBot.Core.Utilities;

namespace Tomat.TomatBot.Content.Commands.Modules
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

                Description = "The following is a lost of all bot commands." +
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
                name += $" ({string.Join(", ", data.Value.CommandDescription)})";

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
                    "`Tomat` is a general-purpose Discord bot programmed by `Tomat#9999` with the help of `TheStachelfisch#0395`, who also hosts." +
                    $"\n\nFor command help, use `{Context.Bot.GetPrefix(Context.Channel)}help` or ping the bot (`@Tomat help`)." +
                    $"\nBot up-time: `{Context.Bot.UpTime.FormatToString(true)}` (since <t:{Context.Bot.StartTime.ToUnixTimeSeconds()}:F>)"
            }.Build());
        }

        #endregion
    }
}