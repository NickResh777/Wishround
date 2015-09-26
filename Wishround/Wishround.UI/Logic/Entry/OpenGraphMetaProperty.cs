using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Wishround.UI.Logic.Entry
{
    public sealed class OpenGraphMetaProperty: PropertyEntryBase{
        private readonly string _property;

        /// <summary>
        /// [og:title], [og:description]
        /// </summary>
        /// <param name="property"></param>
        public OpenGraphMetaProperty(string property){
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
            const string cc = "content=";
            int contentStartIndex = metaTag.IndexOf(cc);

            string result = string.Empty;
            if (contentStartIndex >= 0){
                // skip the OPEN quote symbol
                result = metaTag.Substring((contentStartIndex + cc.Length) + 1);
                
                // skip the CLOSE quote symbol 
                result = result.Substring(0, result.Length - 1);

                int lastEscapedcharIndex = result.Length - 1;
                char currentChar = result.LastOrDefault();
                if (currentChar == 0)
                    return result;

                while (currentChar == '\\' ||
                       currentChar == '\"' ||
                       currentChar == '/'  ||  
                       currentChar == ' '){
                    currentChar = result[lastEscapedcharIndex--];
                }

                result = result.Substring(0, lastEscapedcharIndex);
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