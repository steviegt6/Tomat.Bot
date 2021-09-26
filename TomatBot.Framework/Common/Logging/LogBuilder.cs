#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using Discord;

namespace Tomat.TomatBot.Common.Logging
{
    public class LogBuilder
    {

        public LogSeverity Severity { get; protected set; }

        public string Source { get; protected set; }

        public string Message { get; protected set; }

        public Exception? Exception { get; protected set; }

        public LogBuilder(LogSeverity severity = LogSeverity.Info, string source = "N/A", string message = "",
            Exception? exception = null)
        {
            Severity = severity;
            Source = source;
            Message = message;
            Exception = exception;
        }

        public LogBuilder(LogMessage message) : this(message.Severity, message.Source, message.Message,
            message.Exception)
        {
        }

        public virtual LogBuilder WithSeverity(LogSeverity severity)
        {
            Severity = severity;
            return this;
        }

        public virtual LogBuilder WithSource(string source)
        {
            Source = source;
            return this;
        }

        public virtual LogBuilder WithMessage(string message)
        {
            Message = message;
            return this;
        }

        public virtual LogBuilder WithException(Exception exception)
        {
            Exception = exception;
            return this;
        }

        public virtual LogMessage Build() => new(Severity, Source, Message, Exception);
    }
}