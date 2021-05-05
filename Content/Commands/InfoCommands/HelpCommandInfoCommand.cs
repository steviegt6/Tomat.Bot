using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Tomat.CommandFramework;
using Tomat.CommandFramework.HelpSystem;
using Tomat.Conveniency.Embeds;

namespace TomatBot.Content.Commands.InfoCommands
{
    public sealed class HelpCommandInfoCommand : BaseCommand
    {
        public override MethodInfo? AssociatedMethod => GetType().GetMethod("HandleCommand");

        public override HelpCommandData HelpData =>
            new("commandhelp", "Displays an embed containing extra information about the given command.");

        public override CommandType CType => CommandType.Info;

        public override string Parameters => "<command>";

        [Command("commandhelp")]
        [Summary("Shows info about a given command.")]
        [Alias("chelp", "helpcommand", "helpc")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public Task HandleCommand([Remainder] [Summary("The command.")] string command = "")
        {
            command = command.ToLower();

            HelpCommandData data = CommandRegistry.CommandData.FirstOrDefault(x =>
                x.command == command || x.aliases != null && x.aliases.Contains(command));

            if (!CommandRegistry.CommandData.Contains(data))
                data = new HelpCommandData("error", $"no command with the name {command} found.");

            string description = "";

            if (data.parameters != null)
                description += $"Parameters: {data.parameters}\n";

            description += data.description;

            string name = data.command;

            if (data.aliases != null)
                name += $" ({string.Join(", ", data.aliases)})";

            BaseEmbed embed = new(Context.User)
            {
                Title = name,
                Description = description
            };

            return ReplyAsync(embed: embed.Build());
        }
    }
}