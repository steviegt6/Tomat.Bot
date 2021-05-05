using System;
using System.Threading.Tasks;
using Discord;
using Tomat.Logging.Utilities;

namespace Tomat.Logging
{
    public static class Logger
    {
        private static void PrivateLog(string message, LogLevel level, string source, bool removePadding = false)
        {
            Console.ForegroundColor = ConsoleUtils.ConvertToConsoleColor(level);
            Console.WriteLine(
                $"[{DateTime.UtcNow} (UTC)] [{ConsoleUtils.ConvertLogLevelToString(level)}] [{source}]{(removePadding ? "" : " ")}{message}");
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
        ///     Internal method used for logging Discord.NET messages and awaitable messages.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Task TaskLog(LogMessage message)
        {
            PrivateLog(message.ToString(prependTimestamp: false, padSource: null),
                LoggerUtils.ConvertToLogLevel(message.Severity), message.Source, true);

            return Task.CompletedTask;
        }
    }
}