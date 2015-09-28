using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wishround.UI.Logic.Parsers
{
    public class NullPropertyParser<T> : PropertyParserBase
    {
        protected override object GetValue(){

            return default(T);
        }
    }
}