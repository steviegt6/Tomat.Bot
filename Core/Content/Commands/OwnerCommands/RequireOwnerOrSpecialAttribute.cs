using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace TomatBot.Core.Content.Commands.OwnerCommands
{
    public class RequireOwnerOrSpecialAttribute : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var application = await context.Client.GetApplicationInfoAsync().ConfigureAwait(false);
            
            // TODO: Change this if the hoster changes
            if (context.User.Id != application.Owner.Id || context.User.Id != 442639987180306432) // Id of TheStachelfisch (Current hoster of the bot)
                return PreconditionResult.FromError(ErrorMessage ?? "Command can only be run by the owner of the bot or TheStachelfisch#0395.");
            return PreconditionResult.FromSuccess();
        }
    }
}