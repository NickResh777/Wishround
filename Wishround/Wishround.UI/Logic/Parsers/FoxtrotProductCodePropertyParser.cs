using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Wishround.UI.Logic.Parsers
{
    public class FoxtrotProductCodePropertyParser : PropertyParserBase
    {
        protected override object GetValue(){
            string searchTerm = "<div class=\"article\">";
            int startIndex = Html.IndexOf(searchTerm);

            if (startIndex >= 0){
                string result = Html.Substring(startIndex + searchTerm.Length);

                // skip the text literal (КОД)/Code
                result = SkipCodeKeyword(result);

                // read product code
                result = ReadCode(result);

                return result;
                
            }

            return default(string);
        }

        private string ReadCode(string result){
            result = result.Trim();
            var codeBuilder = new StringBuilder();
            char currentChar = result.FirstOrDefault();
            int currentCharIndex = 0;

            while (Char.IsDigit(currentChar)){
                codeBuilder.Append(currentChar);
                currentChar = result[++currentCharIndex];
            }

            return codeBuilder.ToString();
        }

        private string SkipCodeKeyword(string result){
            int currentCharIndex = 0;
            char currentChar = result.FirstOrDefault();

            while (!Char.IsDigit(currentChar)){
                // move to the next symbol
                currentChar = result[++currentCharIndex];
            }

            return result.Substring((currentCharIndex - 1));
        }
    }
}