using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Tomat.MiscWeb.EitherIO
{
    /// <summary>
    ///     A container for the questions found in a request to http://www.either.io/.
    /// </summary>
    public readonly struct EitherIORequest
    {
        /// <summary>
        ///     Option one. Not set if <see cref="exception"/> isn't empty.
        /// </summary>
        public readonly string optionOne;

        /// <summary>
        ///     Option two. Not set if <see cref="exception"/> isn't empty.
        /// </summary>
        public readonly string optionTwo;

        /// <summary>
        ///     A string describing the error encountered when attempting to fetch the questions from either.io. Normally empty.
        /// </summary>
        public readonly string exception;

        /// <summary>
        ///     Creates an instance of <see cref="EitherIORequest"/> in the scenario that a request was successfully made.
        /// </summary>
        /// <param name="optionOne">Question one.</param>
        /// <param name="optionTwo">Question two.</param>
        public EitherIORequest(string optionOne, string optionTwo)
        {
            this.optionOne = optionOne;
            this.optionTwo = optionTwo;
            exception = "";
        }

        /// <summary>
        ///     Creates an instance of <see cref="EitherIORequest"/> in the scenario that there was an issue when attempting to connect to either.io.
        /// </summary>
        /// <param name="exception"></param>
        public EitherIORequest(string exception)
        {
            optionOne = optionTwo = "";
            this.exception = exception;
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