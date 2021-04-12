using System;
using System.Threading.Tasks;
using Discord;

namespace TomatBot.Core.Logging
{
    public sealed partial class LoggerService //: ServiceBase
    {
        /// <summary>
        ///     Converts a <see cref="LogSeverity"/> to its respective <see cref="LogLevel"/>.
        /// </summary>
        public static LogLevel ConvertToLogLevel(LogSeverity severity)
        {
            return severity switch
            {
                LogSeverity.Critical => LogLevel.Fatal,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Warning => LogLevel.Warn,
                LogSeverity.Info => LogLevel.Info,
                LogSeverity.Verbose => LogLevel.Debug,
                LogSeverity.Debug => LogLevel.Debug,
                _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
            };
        }

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
            switch (level)
            {
                case LogLevel.Info:
                case LogLevel.Warn:
                    return $" {level}";

                default:
                    return $"{level}";
            }
        }

        private static void PrivateLog(string message, LogLevel level, string source, bool removePadding = false)
        {
            Console.ForegroundColor = ConvertToConsoleColor(level);
            Console.WriteLine($"[{DateTime.UtcNow} (UTC)] [{ConvertLogLevelToString(level)}] [{source}]{(removePadding ? "" : " ")}{message}");
        }

        /// <summary>
        ///     Logs a <see cref="LogLevel.Debug"/> message.
        /// </summary>
        public static void Debug(string message) => PrivateLog(message, LogLevel.Debug, "   Self");

        /// <summary>
        ///     Logs a <see cref="LogLevel.Info"/> message.
        /// </summary>
        public static void Info(string message) => PrivateLog(message, LogLevel.Info, "   Self");

        /// <summary>
        ///     Logs a <see cref="LogLevel.Warn"/> message.
        /// </summary>
        public static void Warn(string message) => PrivateLog(message, LogLevel.Warn, "   Self");

        /// <summary>
        ///     Logs a <see cref="LogLevel.Error"/> message.
        /// </summary>
        public static void Error(string message) => PrivateLog(message, LogLevel.Error, "   Self");

        /// <summary>
        ///     Logs a <see cref="LogLevel.Fatal"/> message.
        /// </summary>
        public static void Fatal(string message) => PrivateLog(message, LogLevel.Fatal, "   Self");

        /// <summary>
        ///     Internal method used for logging Discord.NET messages and awaitable messages;
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static Task TaskLog(LogMessage message)
        {
            PrivateLog(message.ToString(prependTimestamp: false, padSource: null), ConvertToLogLevel(message.Severity), message.Source, true);

            return Task.CompletedTask;
        }
    }
}