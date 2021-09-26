#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Tomat.TomatBot.Common.Web.EitherIO
{
    public class EitherIORequest
    {
        public readonly (string OptionOne, string OptionTwo) Options;

        /// <summary>
        ///     A string describing the error encountered when attempting to fetch the questions from either.io. Normally empty.
        /// </summary>
        public readonly string Exception;

        /// <summary>
        ///     Creates an instance of <see cref="EitherIORequest"/> in the scenario that a request was successfully made.
        /// </summary>
        /// <param name="optionOne">Question one.</param>
        /// <param name="optionTwo">Question two.</param>
        public EitherIORequest(string optionOne, string optionTwo)
        {
            Options = (optionOne, optionTwo);
            Exception = "";
        }

        /// <summary>
        ///     Creates an instance of <see cref="EitherIORequest"/> in the scenario that there was an issue when attempting to connect to either.io.
        /// </summary>
        /// <param name="exception"></param>
        public EitherIORequest(string exception)
        {
            Exception = exception;
        }

        /// <summary>
        ///     Takes care of the process of making a request to either.io and searching for the two options.
        /// </summary>
        /// <returns>An instance of <see cref="EitherIORequest"/>. Check to make sure <see cref="exception"/> to empty.</returns>
        public static EitherIORequest MakeRequest()
        {
            HtmlWeb webClient = new();
            HtmlDocument? htmlDoc = webClient.Load(@"http://www.either.io/");

            if (htmlDoc == null)
                return new EitherIORequest("HTML document was null!");

            List<HtmlNode> classes = htmlDoc.DocumentNode.Descendants("span")
                .Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("option-text"))
                .ToList();

            return classes.Count < 2
                ? new EitherIORequest("Could not find a minimum of two classes!")
                : new EitherIORequest(classes[0].InnerText, classes[1].InnerText);
        }
    }
}