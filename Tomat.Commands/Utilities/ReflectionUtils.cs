using System;
using System.Linq;
using System.Reflection;
using Discord.Commands;

namespace Tomat.CommandFramework.Utilities
{
    public static class ReflectionUtils
    {
        /// <summary>
        ///     Gets the requested <see cref="Attribute"/> in accordance to <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of <see cref="Attribute"/> to search for, must extend <see cref="Attribute"/>.</typeparam>
        /// <param name="info">The <see cref="MethodInfo"/> instance to search.</param>
        /// <returns>A nullable <typeparamref name="TAttribute"/>. Will have a value if the <see cref="Attribute"/> was successfully found.</returns>
        public static TAttribute? GetCustomAttribute<TAttribute>(this MethodInfo info) where TAttribute : Attribute =>
            info.GetCustomAttributes().FirstOrDefault(x => x.GetType() == typeof(TAttribute)) as TAttribute;

        /// <summary>
        ///     Gets the requested <see cref="Attribute"/> in accordance to <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="Type"/> of <see cref="Attribute"/> to search for, must extend <see cref="Attribute"/>.</typeparam>
        /// <param name="type">The <see cref="Type"/> instance to search.</param>
        /// <returns>A nullable <typeparamref name="TAttribute"/>. Will have a value if the <see cref="Attribute"/> was successfully found.</returns>
        public static TAttribute? GetCustomAttribute<TAttribute>(this Type type) where TAttribute : Attribute =>
            type.GetCustomAttributes().FirstOrDefault(x => x.GetType() == typeof(TAttribute)) as TAttribute;

        // Self-explanatory
        public static string? CommandNameFromAttribute(this MethodInfo info) =>
            info.GetCustomAttribute<CommandAttribute>()?.Text;

        // Self-explanatory
        public static string? CommandSummaryFromAttribute(this MethodInfo info) =>
            info.GetCustomAttribute<SummaryAttribute>()?.Text;

        // Self-explanatory
        public static string[]? CommandAliasesFromAttribute(this MethodInfo info) =>
            info.GetCustomAttribute<AliasAttribute>()?.Aliases;
    }
}
