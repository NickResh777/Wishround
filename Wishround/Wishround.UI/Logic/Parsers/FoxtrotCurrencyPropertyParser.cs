using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Wishround.UI.Logic.Parsers;

namespace Wishround.UI.Logic.Entry
{
    public class FoxtrotCurrencyPropertyParser: PropertyParserBase
    {
        protected override object GetValue()
        {
            string search = "<t class=\"curr_price\">";

            int tagIndex = Html.IndexOf(search);

            if (tagIndex >= 0){
                string result = Html.Substring(tagIndex + search.Length);
                int currentSpanIndex = result.IndexOf("<span>");
                if (currentSpanIndex >= 0){
                    result = result.Substring(currentSpanIndex + "<span>".Length);
                    result = ReadCurrency(result);
                }

                return result;
            }

            return string.Empty;
        }

        private string ReadCurrency(string result){
            var currencyBuilder = new StringBuilder();
            char currentChar = result.FirstOrDefault();
            int currentCharIndex = 0;

            while (currentChar != '<'){
                currencyBuilder.Append(currentChar);
                currentChar = result[++currentCharIndex];
            }
            return currencyBuilder.ToString();
        }

    }
}