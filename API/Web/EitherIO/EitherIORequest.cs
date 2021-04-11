using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace TomatBot.API.Web.EitherIO
{
    public readonly struct EitherIORequest
    {
        public readonly string optionOne;
        public readonly string optionTwo;
        public readonly string exception;

        public EitherIORequest(string optionOne, string optionTwo)
        {
            this.optionOne = optionOne;
            this.optionTwo = optionTwo;
            exception = "";
        }

        public EitherIORequest(string exception)
        {
            optionOne = optionTwo = "";
            this.exception = exception;
        }

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