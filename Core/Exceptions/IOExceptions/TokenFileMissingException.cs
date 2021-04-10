using System;

namespace TomatBot.Core.Exceptions.IOExceptions
{
    public class TokenFileMissingException : Exception
    {
        public override string Message =>
            "token.txt file not found!" +
            "\nYou'll have to supply the program with your own token.txt file containing only your bot's token" +
            " and place this in the same folder as the bot's generated .exe file." +
            "\nThis allows the bot to stay open-source and open for contributions.";

        public override string HelpLink => "https://github.com/Steviegt6/TomatBot/wiki/TokenFileMissingException";
    }
}