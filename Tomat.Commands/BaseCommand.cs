using System.Reflection;
using Discord.Commands;
using Tomat.CommandFramework.HelpSystem;
using Tomat.CommandFramework.Utilities;

namespace Tomat.CommandFramework
{
    public abstract class BaseCommand : ModuleBase<SocketCommandContext>
    {
        public virtual string? Name => AssociatedMethod?.CommandNameFromAttribute();

        public virtual string? CommandSummary => AssociatedMethod?.CommandSummaryFromAttribute();

        public virtual string[]? Aliases => AssociatedMethod?.CommandAliasesFromAttribute();

        public virtual string? Parameters => null;

        public abstract MethodInfo? AssociatedMethod { get; }

        public abstract HelpCommandData HelpData { get; }

        public abstract CommandType CType { get; }
    }
}
