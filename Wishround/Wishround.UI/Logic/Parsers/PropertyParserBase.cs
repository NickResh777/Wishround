using System;

namespace Wishround.UI.Logic.Parsers
{
    public abstract class PropertyParserBase{
        private bool   _valueSet;
        private object _value;
        private Uri    _url;
        private string _html;


        public string Html{
            get{
                return _html;
            }
            set { 
                _html = value; 
            }
        }

        public Uri Uri{
            get{
                return _url;
            }
            set { 
                _url = value; }
        }

        public object Value{
            get{
                if (_valueSet)
                    return _value;
                _value = GetValue();
                _valueSet = true;
                return _value;
            }
        }

        protected abstract object GetValue();
    }
}