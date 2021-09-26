#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using System.Text;

namespace Tomat.TomatBot.Core.Utilities
{
    public static class TimeSpanUtilities
    {
        /// <summary>
        ///     Formats the given <paramref name="timeSpan"/>.
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> instance to format.</param>
        /// <param name="shorten">Whether or not the abbreviate days, hours, etc. to d, h, etc.</param>
        /// <returns>The newly-created, formatted string.</returns>
        public static string FormatToString(this TimeSpan timeSpan, bool shorten)
        {
            if (timeSpan == TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan), "Timespan may not be Zero");

            StringBuilder builder = new StringBuilder()
                .AppendTimeSpanValue(timeSpan.Days, "Day", "Days", "d", shorten)
                .AppendTimeSpanValue(timeSpan.Hours, "Hour", "Hours", "h", shorten)
                .AppendTimeSpanValue(timeSpan.Minutes, "Minute", "Minutes", "m", shorten)
                .AppendTimeSpanValue(timeSpan.Seconds, "Second", "Seconds", "s", shorten);

            return builder.Remove(builder.Length - 1, 1).ToString();
        }

        /// <summary>
        ///     Generates part of a string corresponding to the given amount and provided words.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to append to.</param>
        /// <param name="amount">The count to add. This should be something like <see cref="TimeSpan.Days"/>.</param>
        /// <param name="fullSingle">The full word in a singular context (i.e. "Day").</param>
        /// <param name="fullPlural">The full word in a plural context (i.e. "Days").</param>
        /// <param name="shortBoth">The abbreviated word or letter to use in both contexts, provided <see cref="shouldShorten"/> is <c>true</c> (i.e. "d").</param>
        /// <param name="shouldShorten">Whether or not <paramref name="shortBoth"/> should be used in favor of <paramref name="fullSingle"/> and <paramref name="fullPlural"/>.</param>
        /// <returns>The same <see cref="StringBuilder"/> instance (<paramref name="builder"/>), for MethodBuilders.</returns>
        private static StringBuilder AppendTimeSpanValue(this StringBuilder builder, int amount, string fullSingle,
            string fullPlural, string shortBoth, bool shouldShorten)
        {
            if (amount == 0)
                return builder;

            string result = amount == 1
                ? $"{amount}{(shouldShorten ? shortBoth : $" {fullSingle}")}"
                : $"{amount}{(shouldShorten ? shortBoth : $" {fullPlural}")}";

            return builder.Append($"{result} ");
        }
    }
}