#nullable enable

using System;

namespace TomatBot.Core.Exceptions.IOExceptions
{
    public class TokenFileMissingException : Exception
    {
        public override string Message =>
            "token.txt file not found!" +
            "\nYou'll have to supply the program with your own token.txt file container only your bot's token" +
            " and place this in the same folder and the bot's generated .exe file." +
            "\nThis allows this bot to stay open-source and open for contributions.";

        // TODO: HelpLink
        public override string? HelpLink => null;
    }
}

#nullable disable