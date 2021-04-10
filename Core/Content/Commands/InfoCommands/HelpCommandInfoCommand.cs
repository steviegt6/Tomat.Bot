using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TomatBot.Core.Content.Embeds;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;

namespace TomatBot.Core.Content.Commands.InfoCommands
{
    public sealed class HelpCommandInfoCommand : TomatCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData => new HelpCommandData("commandhelp", "Displays an embed containing extra information about the given command.");

        public override CommandType CType => CommandType.Info;

        public override string Parameters => "<command>";

        [Command("commandhelp")]
        [Summary("Shows info about a given command.")]
        [Alias("chelp", "helpcommand", "helpc")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public Task HandleCommand([Remainder]
            [Summary("The command.")]
            string command = "")
        {
            command = command.ToLower();

            // TODO: alias support
            HelpCommandData data = CommandRegistry.helpCommandData!.FirstOrDefault(x => x.command == command || x.aliases != null && x.aliases.Contains(command));

            if (!CommandRegistry.helpCommandData!.Contains(data))
                data = new HelpCommandData("error", $"no command with the name {command} found.");

            string description = "";

            if (data.parameters != null)
                description += $"Parameters: {data.parameters}\n";

            description += data.description;

            string name = data.command;

            if (data.aliases != null)
                name += $" ({string.Join(", ", data.aliases)})";

            BaseEmbed embed = new BaseEmbed(Context.User)
            {
                Title = name,
                Description = description
            };

            return ReplyAsync(embed: embed.Build());
        }
    }
}
