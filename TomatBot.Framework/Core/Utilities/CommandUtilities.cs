#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using Discord.Commands;
using Discord.WebSocket;
using Tomat.TomatBot.Core.Bot;

namespace Tomat.TomatBot.Core.Utilities
{
    public static class CommandUtilities
    {
        public enum InvalidMessageReason
        {
            NoPrefixOrMention,
            AuthorIsWebhook,
            AuthorIsBot,
            NotUserMessage,
            NoError
        }

        /// <summary>
        ///     Checks whether or not a <see cref="SocketMessage"/> instance acts as a valid bot mention.
        /// </summary>
        /// <param name="message">The message to verify.</param>
        /// <param name="bot"></param>
        /// <param name="invalidReason">The reason for failure, if applicable.</param>
        /// <param name="argumentPosition">Position of the referenced argument.</param>
        /// <param name="mentionClient">Instance of a <see cref="DiscordSocketClient"/> if you want to check for mention prefixes.</param>
        /// <returns>Whether the message is valid.</returns>
        public static bool ValidateMessageMention(this SocketMessage message, DiscordBot bot,
            out InvalidMessageReason invalidReason, out int argumentPosition,
            DiscordSocketClient? mentionClient = null)
        {
            argumentPosition = 0;

            if (!message.ToSocketUserMessage(out SocketUserMessage? userMessage, out invalidReason))
                return false;

            return userMessage!.CheckAutomaton(out invalidReason)
                   && HasValidPrefix(userMessage!, bot, out invalidReason, out argumentPosition, mentionClient);
        }

        /// <summary>
        ///     Checks for valid prefixes for a message.
        /// </summary>
        /// <param name="message">The message to check.</param>
        /// <param name="bot"></param>
        /// <param name="invalidReason">The reason for failure, if applicable.</param>
        /// <param name="argumentPosition">Position of the reference argument.</param>
        /// <param name="mentionClient">Instance of a <see cref="DiscordSocketClient"/> if you want to check for mention prefixes.</param>
        /// <returns>Whether the message is validly prefixed.</returns>
        public static bool HasValidPrefix(SocketUserMessage message, DiscordBot bot,
            out InvalidMessageReason invalidReason, out int argumentPosition,
            DiscordSocketClient? mentionClient = null)
        {
            argumentPosition = 0;
            invalidReason = InvalidMessageReason.NoError;

            if (message.HasStringPrefix(bot.GetPrefix(message.Channel), ref argumentPosition,
                StringComparison.OrdinalIgnoreCase))
                return true;

            if (mentionClient != null && message.HasMentionPrefix(mentionClient.CurrentUser, ref argumentPosition))
                return true;

            invalidReason = InvalidMessageReason.NoPrefixOrMention;
            return false;
        }

        /// <summary>
        ///     Checks whether or not a <see cref="SocketUserMessage"/> is valid according to the specified parameters.
        /// </summary>
        /// <param name="message">The message to check.</param>
        /// <param name="invalidReason">The reason for failure, if applicable.</param>
        /// <param name="allowBots">Whether bots should be listened to.</param>
        /// <param name="allowWebhooks">Whether webhooks should be listened to.</param>
        /// <returns>True if successfully passed checks.</returns>
        public static bool CheckAutomaton(this SocketUserMessage message, out InvalidMessageReason invalidReason,
            bool allowBots = false, bool allowWebhooks = false)
        {
            if (!allowBots && message.Author.IsBot)
            {
                invalidReason = InvalidMessageReason.AuthorIsBot;
                return false;
            }

            if (!allowWebhooks && message.Author.IsWebhook)
            {
                invalidReason = InvalidMessageReason.AuthorIsWebhook;
                return false;
            }

            invalidReason = InvalidMessageReason.NoError;
            return true;
        }

        /// <summary>
        ///     Converts a <see cref="SocketMessage"/> to a <see cref="SocketUserMessage"/>.
        /// </summary>
        /// <param name="message">The message to convert.</param>
        /// <param name="userMessage">The converted message.</param>
        /// <returns>True if successfully converted.</returns>
        public static bool ToSocketUserMessage(this SocketMessage message, out SocketUserMessage? userMessage)
        {
            userMessage = null;

            if (message is not SocketUserMessage socketUserMessage)
                return false;

            userMessage = socketUserMessage;
            return true;

        }

        /// <summary>
        ///     Converts a <see cref="SocketMessage"/> to a <see cref="SocketUserMessage"/>, and provides a <see cref="InvalidMessageReason"/> on failure.
        /// </summary>
        /// <param name="message">The message to convert.</param>
        /// <param name="userMessage">The converted message.</param>
        /// <param name="invalidReason">Reason for failure (if applicable).</param>
        /// <returns>True if successfully converted.</returns>
        public static bool ToSocketUserMessage(this SocketMessage message, out SocketUserMessage? userMessage,
            out InvalidMessageReason invalidReason)
        {
            if (message.ToSocketUserMessage(out userMessage))
            {
                invalidReason = InvalidMessageReason.NoError;
                return true;
            }

            invalidReason = InvalidMessageReason.NotUserMessage;
            return false;
        }
    }
}