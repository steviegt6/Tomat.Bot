#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Tomat.TomatBot.Common.Embeds;
using Tomat.TomatBot.Core.CommandContext;
using Tomat.TomatBot.Core.Services.Commands;

namespace Tomat.TomatBot.Content.Modules.Commands.General
{
    [ModuleInfo("Owner-Specific", false)]
    public sealed class OwnerModule : ModuleBase<BotCommandContext>
    {
        public class RequireAuthorizedUserAttribute : PreconditionAttribute
        {
            private const ulong Pollen = 342472993567408148;

            public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
                CommandInfo command, IServiceProvider services)
            {
                IApplication? application = await context.Client.GetApplicationInfoAsync().ConfigureAwait(false);

                if (context.User.Id == application.Owner.Id || context.User.Id == Pollen)
                    return PreconditionResult.FromSuccess();

                return PreconditionResult.FromError(
                    ErrorMessage ?? "Command can only be run by an **authorized user**.");
            }
        }

        #region Forced Stuff

        [RequireAuthorizedUser]
        [Command("forceload")]
        [Alias("fl")]
        [Summary("Forcefully loads all configs.")]
        public async Task ForceLoadAsync()
        {
            await Context.Bot.LoadConfig();

            await ReplyAsync(embed: EmbedHelper.SuccessEmbed("Loaded configuration files!",
                Context.Bot.CreateSmallEmbed(Context.User).Footer));
        }

        [RequireAuthorizedUser]
        [Command("forcesave")]
        [Alias("fs")]
        [Summary("Forcefully saves all configs.")]
        public async Task ForceSaveAsync()
        {
            await Context.Bot.SaveConfig();

            await ReplyAsync(embed: EmbedHelper.SuccessEmbed("Saved configuration files!",
                Context.Bot.CreateSmallEmbed(Context.User).Footer));
        }

        #endregion
    }
}