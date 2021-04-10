using System.Reflection;
using Discord.Commands;
using TomatBot.Core.Content.Embeds;
using TomatBot.Core.Framework.DataStructures;
using TomatBot.Core.Utilities;

namespace TomatBot.Core.Framework.CommandFramework
{
    public abstract class TomatCommand : ModuleBase<SocketCommandContext>
    {
        public virtual string? Name => AssociatedMethod?.CommandNameFromAttribute();

        public virtual string? CommandSummary => AssociatedMethod?.CommandSummaryFromAttribute();

        public virtual string[]? Aliases => AssociatedMethod?.CommandAliasesFromAttribute();

        public virtual string? Parameters => null;

        public abstract MethodInfo? AssociatedMethod { get; }

        public abstract HelpCommandData HelpData { get; }

        public abstract CommandType CType { get; }

        public BaseEmbed CreateSmallEmbed(string text = "") =>
            new BaseEmbed(Context.User)
            {
                Description =
                    string.IsNullOrEmpty(text)
                        ? "I'm a bot, and this action was performed automatically. If you have a problem with that, go back to Reddit."
                        : text
            };
    }
}
