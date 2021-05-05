using System;
using Discord;

namespace Tomat.Logging.Utilities
{
    public class LoggerUtils
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
    }
}