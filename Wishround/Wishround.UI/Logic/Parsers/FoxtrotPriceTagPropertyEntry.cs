using System;
using System.Linq;
using System.Text;

namespace Wishround.UI.Logic.Parsers
{
    public class FoxtrotPriceTagPropertyEntry : PropertyParserBase
    {
        protected override object GetValue()
        {
            string searchTag = "<t class=\"curr_price\">";
            int tagT = Html.IndexOf(searchTag);

            if (tagT >= 0){
                string result = Html.Substring(tagT + searchTag.Length);
                string priceTag = ReadPrice(result);

                return Decimal.Parse(priceTag);
            }

            return (decimal) 0;
        }

        private string ReadPrice(string priceTag){

            var priceBuilder = new StringBuilder();
            char currentChar = priceTag.FirstOrDefault();

            int charIndex = 0;


            while (Char.IsDigit(currentChar)){
                priceBuilder.Append(currentChar);
                currentChar = priceTag[++charIndex];
            }

        

            return priceBuilder.ToString();
        }
    }
}