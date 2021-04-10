using System;
using System.Linq;
using System.Reflection;
using Discord.Commands;

namespace TomatBot.Core.Utilities
{
    public static class ReflectionHelper
    {
        public static TAttribute? GetCustomAttribute<TAttribute>(this MethodInfo info) where TAttribute : Attribute =>
            info.GetCustomAttributes().FirstOrDefault(x => x.GetType() == typeof(TAttribute)) as TAttribute;

        public static TAttribute? GetCustomAttribute<TAttribute>(this Type type) where TAttribute : Attribute =>
            type.GetCustomAttributes().FirstOrDefault(x => x.GetType() == typeof(TAttribute)) as TAttribute;

        public static string? CommandNameFromAttribute(this MethodInfo info) =>
            info.GetCustomAttribute<CommandAttribute>()?.Text;

        public static string? CommandSummaryFromAttribute(this MethodInfo info) =>
            info.GetCustomAttribute<SummaryAttribute>()?.Text;

        public static string[]? CommandAliasesFromAttribute(this MethodInfo info) =>
            info.GetCustomAttribute<AliasAttribute>()?.Aliases;
    }
}
