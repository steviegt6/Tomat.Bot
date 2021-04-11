using System;
using System.Text;

namespace TomatBot.Core.Utilities
{
    // https://github.com/TheStachelfisch/tmod64-Bot/blob/723a678520f40147d57c9e3cc5b4f07ef4950153/tMod64Bot/Utils/TimeSpanUtils.cs#L6
    public static class TimeSpanUtils
    {
        public static string FormatToString(this TimeSpan timeSpan, bool shorten)
        {
            if (timeSpan == TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan), "Timespan may not be Zero");

            StringBuilder builder = new();

            if (timeSpan.Days != 0)
                builder.Append($"{(timeSpan.Days == 1 ? $"{timeSpan.Days}{(shorten ? "d" : " Day")}" : $"{timeSpan.Days}{(shorten ? "d" : " Days")}")} ");
            if (timeSpan.Hours != 0)
                builder.Append($"{(timeSpan.Hours == 1 ? $"{timeSpan.Hours}{(shorten ? "h" : " Hour")}" : $"{timeSpan.Hours}{(shorten ? "h" : " Hours")}")} ");
            if (timeSpan.Minutes != 0)
                builder.Append($"{(timeSpan.Minutes == 1 ? $"{timeSpan.Minutes}{(shorten ? "m" : " Minute")}" : $"{timeSpan.Minutes}{(shorten ? "m" : " Minutes")}")} ");
            if (timeSpan.Seconds != 0)
                builder.Append($"{(timeSpan.Seconds == 1 ? $"{timeSpan.Seconds}{(shorten ? "s" : " Second")}" : $"{timeSpan.Seconds}{(shorten ? "s" : " Seconds")}")}");

            return builder.ToString();
        }
    }
}