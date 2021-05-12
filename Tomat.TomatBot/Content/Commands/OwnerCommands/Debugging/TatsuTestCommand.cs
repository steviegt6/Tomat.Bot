using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Tomat.CommandFramework;
using Tomat.CommandFramework.HelpSystem;
using Tomat.Conveniency.Embeds;
using Tomat.Conveniency.Utilities;
using Tomat.TatsuSharp;
using Tomat.TatsuSharp.Data;
using Tomat.TomatBot.Exceptions.IOExceptions;

namespace Tomat.TomatBot.Content.Commands.OwnerCommands.Debugging
{
    public class TatsuTestCommand : BaseCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("Test");

        public override HelpCommandData HelpData => new("", "");

        public override CommandType CType => CommandType.Hidden;

        [RequireOwnerOrSpecial]
        [Command("tatsutest")]
        [Summary("")]
        public async Task Test(bool rateLimit)
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
    }
}