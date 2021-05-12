using System;

namespace Tomat.TomatBot.Exceptions.IOExceptions
{
    public class TatsuAPIFileMissingException : Exception
    {
        public override string Message =>
            "tatsu.txt file not found!" +
            "\nYou'll have to supply the program with your own tatsu.txt file containing only your Tatsu API" +
            " and place this in the same folder as the bot's generated .exe file." +
            "\nThis allows the bot to stay open-source and open for contributions.";

        // public override string HelpLink => "https://github.com/TomatCord/TomatBot/wiki/TokenFileMissingException";
    }
}