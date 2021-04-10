using System;
using System.Threading.Tasks;
using Discord;

namespace TomatBot.Core.Logging
{
    internal static class Logger
    {
        /// <summary>
        /// Converts a <see cref="LogSeverity"/> to its respective <see cref="LogLevel"/>.
        /// </summary>
        private static LogLevel ConvertToLogLevel(LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Critical:
                    return LogLevel.Fatal;

                case LogSeverity.Error:
                    return LogLevel.Error;

                case LogSeverity.Warning:
                    return LogLevel.Warn;

                case LogSeverity.Info:
                    return LogLevel.Info;

                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    return LogLevel.Debug;

                default:
                    throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
            }
        }

        private static void PrivateLog(string message, LogLevel level) =>
            Console.WriteLine($"[{DateTime.UtcNow} (UTC)] [{level}] {message}");

        /// <summary>
        /// Logs a <see cref="LogLevel.Debug"/> message.
        /// </summary>
        public static void Debug(string message) => PrivateLog(message, LogLevel.Debug);

        /// <summary>
        /// Logs a <see cref="LogLevel.Info"/> message.
        /// </summary>
        public static void Info(string message) => PrivateLog(message, LogLevel.Info);

        /// <summary>
        /// Logs a <see cref="LogLevel.Warn"/> message.
        /// </summary>
        public static void Warn(string message) => PrivateLog(message, LogLevel.Warn);

        /// <summary>
        /// Logs a <see cref="LogLevel.Error"/> message.
        /// </summary>
        public static void Error(string message) => PrivateLog(message, LogLevel.Error);

        /// <summary>
        /// Logs a <see cref="LogLevel.Fatal"/> message.
        /// </summary>
        public static void Fatal(string message) => PrivateLog(message, LogLevel.Fatal);

        internal static Task TaskLog(LogMessage message)
        {
            PrivateLog(message.ToString(), ConvertToLogLevel(message.Severity));

            return Task.CompletedTask;
        }
    }
}