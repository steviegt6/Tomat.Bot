using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Tomat.Conveniency.Embeds;
using Tomat.Conveniency.Utilities;
using Tomat.TatsuSharp;
using Tomat.TatsuSharp.Data;
using Tomat.TomatBot.Content.Services;
using Tomat.TomatBot.Exceptions.IOExceptions;
using Tomat.TomatBot.Utilities;

namespace Tomat.TomatBot.Content.Commands
{
    public sealed class OwnerModule : ModuleBase<SocketCommandContext>
    {
        public class RequireOwnerOrSpecialAttribute : PreconditionAttribute
        {
            private const ulong StachelID = 442639987180306432;

            public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
                CommandInfo command, IServiceProvider services)
            {
                IApplication? application = await context.Client.GetApplicationInfoAsync().ConfigureAwait(false);

                // Change this if the hoster changes
                if (context.User.Id == application.Owner.Id || context.User.Id == StachelID)
                    return PreconditionResult.FromSuccess();

                return PreconditionResult.FromError(ErrorMessage ??
                                                    "Command can only be run by **the owner of the bot** or `TheStachelfisch#0395`.");
            }
        }

        #region Forced Stuff

        [RequireOwnerOrSpecial]
        [Command("forceload")]
        [Alias("fl")]
        [Summary("Forcefully loads all configs.")]
        public async Task ForceLoadAsync()
        {
            await BotStartup.Provider.GetRequiredService<ConfigService>().Config.LoadConfigs();

            await ReplyAsync(embed: EmbedHelper.SuccessEmbed("Loaded configuration files!",
                EmbedHelper.CreateSmallEmbed(Context.User).Footer));
        }

        [RequireOwnerOrSpecial]
        [Command("forcesave")]
        [Alias("fs")]
        [Summary("Forcefully saves all configs.")]
        public async Task ForceSaveAsync()
        {
            BotStartup.Provider.GetRequiredService<ConfigService>().Config.SaveConfigs();

            Embed embed = EmbedHelper.SuccessEmbed("Saved configuration files!",
                EmbedHelper.CreateSmallEmbed(Context.User).Footer);
            await ReplyAsync(embed: embed);
        }

        #endregion

        #region Update Command

        [RequireOwnerOrSpecial]
        [Command("update")]
        [Summary("Updates the bot.")]
        public async Task UpdateAsync()
        {
            if (!File.Exists("../TomatUpdate.bash"))
            {
                await ReplyAsync(
                    $"TomatUpdate.bash file doesn't exist ({MentionUtils.MentionUser(442639987180306432)})");
                return;
            }

            IUserMessage? message = await ReplyAsync(embed: new EmbedBuilder
            {
                Title = "Command output",
                Description = "Starting command"
            }.Build());

            await File.WriteAllTextAsync("Restarted.txt", $"{Context.Guild.Id} {Context.Channel.Id}");

            // If there is something to update the bot will just exit
            string result = "../TomatUpdate.bash".Bash();

            await message.ModifyAsync(x => x.Embed = new EmbedBuilder
            {
                Title = "Command output",
                Description = result
            }.Build());

            File.Delete("Restarted.txt");
        }

        #endregion

        #region DEBUGGING lol

        [RequireOwnerOrSpecial]
        [Command("tatsutest")]
        [Summary("")]
        public async Task TatsuTestAsync(bool rateLimit)
        {
            if (!File.Exists("tatsu.txt"))
                throw new TatsuAPIFileMissingException();

            await ReplyAsync("Creating Tatsu client...");
            TatsuClient client = new(await File.ReadAllTextAsync("tatsu.txt"));
            await ReplyAsync("Created Tatsu client!");
            await ReplyAsync("Displaying profile information...");

            try
            {
                if (rateLimit)
                {
                    while (true)
                    {
                        Console.WriteLine(client.Bucket.Remaining);
                        Console.WriteLine(client.Bucket.ResetInterval);
                        Console.WriteLine(client.Bucket.ResetTime);
                        Console.WriteLine(DateTime.Now);
                        await client.GetUserProfile(Context.User.Id.ToString());
                    }
                }

                TatsuUser user = await client.GetUserProfile(Context.User.Id.ToString());

                await ReplyAsync(embed: new BaseEmbed(Context.User)
                {
                    Title = user.Title,
                    Description = $"Avatar URL: {user.AvatarURL}" +
                                  $"\nCredits: {user.Credits}" +
                                  $"\nDiscriminator: {user.Discriminator}" +
                                  $"\nID: {user.ID}" +
                                  $"\nInfo box: {user.InfoBox}" +
                                  $"\nReputation: {user.Reputation}" +
                                  $"\nTokens: {user.Tokens}" +
                                  $"\nUsername: {user.Username}" +
                                  $"\nXP: {user.XP}"
                }.Build());

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                // TODO: actual exception type lol
                if (e.Message.Equals("Rate-limited."))
                    await ReplyAsync(embed: EmbedHelper.ErrorEmbed("Tatsu API rate limit exceeded!"));
            }
        }

        #endregion
    }
}