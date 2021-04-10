using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TomatBot.Core.Content.Embeds;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;

namespace TomatBot.Core.Content.Commands.InfoCommands
{
    public sealed class HelpCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData => new HelpCommandData("help", "Displays an embed containing basic information on all registered commands.");

        public override CommandType CType => CommandType.Info;

        [Command("help")]
        [Summary("Shows bot commands.")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public Task HandleCommand()
        {
            string listOfInfoCommands = string.Join('\n', CommandRegistry.infoCommands!);
            string listOfFunCommands = string.Join('\n', CommandRegistry.funCommands!);

            BaseEmbed embed = new BaseEmbed(Context.User)
            {
                Title = "Command Help",

                Description = "The following is a list of all bot commands." +
                              $"\nAll of these should be prefixed with {BotStartup.Prefix}." +
                              "\n`<>`: required" +
                              "\n`[]`: optional",
                Fields = new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        IsInline = false,
                        Name = "Informative Commands",
                        Value = string.IsNullOrEmpty(listOfInfoCommands) ? "N/A" : listOfInfoCommands
                    },

                    new EmbedFieldBuilder
                    {
                        IsInline = false,
                        Name = "Fun Commands",
                        Value = string.IsNullOrEmpty(listOfFunCommands) ? "N/A" : listOfFunCommands
                    }
                }

            };

            return ReplyAsync(embed: embed.Build());
        }
    }
}
