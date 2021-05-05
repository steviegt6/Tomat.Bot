using System;

namespace Tomat.Logging.Utilities
{
    public static class ConsoleUtils
    {
        /// <summary>
        ///     Finds the <see cref="ConsoleColor"/> associated with the provided <see cref="LogLevel"/> (<paramref name="level"/>).
        /// </summary>
        /// <param name="level">The <see cref="LogLevel"/> to check.</param>
        /// <returns>The associated <see cref="ConsoleColor"/>.</returns>
        public static ConsoleColor ConvertToConsoleColor(LogLevel level)
        {
            return level switch
            {
                LogLevel.Debug => ConsoleColor.DarkGray,
                LogLevel.Info => ConsoleColor.White,
                LogLevel.Warn => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Fatal => ConsoleColor.DarkRed,
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
            };
        }

        /// <summary>
        ///     Applies extra spaces to the name of a <see cref="LogLevel"/> to make everything line up in mono-space logs.
        /// </summary>
        /// <param name="level">The <see cref="LogLevel"/> to modify.</param>
        /// <returns>A "formatted" string.</returns>
        public static string ConvertLogLevelToString(LogLevel level)
        {
            return level switch
            {
                LogLevel.Info => $" {level}",
                LogLevel.Warn => $" {level}",
                _ => $"{level}"
            };
        }
    }
}