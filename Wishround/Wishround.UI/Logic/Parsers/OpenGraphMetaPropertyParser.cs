using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using Wishround.UI.Logic.Entry;

namespace Wishround.UI.Logic.Parsers
{
    public sealed class OpenGraphMetaPropertyParser: PropertyParserBase{
        private readonly string _property;

        /// <summary>
        /// [og:title], [og:description]
        /// </summary>
        /// <param name="property"></param>
        public OpenGraphMetaPropertyParser(string property){
            _property = property;
        }

        public string Property{
            get{
                return _property;
            }
        }

        protected override object GetValue(){
            string ogProperty = string.Format("<meta property=\"{0}\"", Property);
            int startIndex = Html.IndexOf(ogProperty);
            if (startIndex >= 0){
                // not found
                string metaTag = ReadUntilClosingTag(startIndex);
                return GetContentValue(new HtmlString(metaTag).
                                             ToHtmlString());
            }

            // return just an empty string
            return string.Empty;
        }

        private string GetContentValue(string metaTag){
            const string contentAttr = "content=";
            int contentStartIndex = metaTag.IndexOf(contentAttr);

            string result = string.Empty;
            if (contentStartIndex >= 0){
                // skip the OPEN quote symbol
                result = metaTag.Substring((contentStartIndex + contentAttr.Length) + 1);
                
              

                int lastEscapedCharIndex = result.Length - 1;
                char currentChar = result.LastOrDefault();
                if (currentChar == 0)
                    return result;

                while (currentChar == '\\' ||
                       currentChar == '\"' ||
                       currentChar == '/'  ||  
                       currentChar == '>' ||
                       currentChar == ' '){
                    currentChar = result[lastEscapedCharIndex--];
                }

                result = result.Substring(0, (lastEscapedCharIndex + 2));
            }

            return result;
        }

        private string ReadUntilClosingTag(int startIndex){
            var metaTagBuilder = new StringBuilder();

           
            string metaHtml =  new HtmlString(Html.Substring(startIndex)).ToHtmlString();
            char currentSymbol = metaHtml.First();
            int charIndex = 0;

            while (currentSymbol != '>'){
                if (charIndex == 0){
                    // assert the first symbol should be a valid 
                    // OPENING tag symbol
                    Debug.Assert(metaHtml.First() == '<');
                }

                metaTagBuilder.Append(currentSymbol);
                currentSymbol = metaHtml[++charIndex];
            }

            metaTagBuilder.Append(">");
            return metaTagBuilder.ToString();
        }
    }
}