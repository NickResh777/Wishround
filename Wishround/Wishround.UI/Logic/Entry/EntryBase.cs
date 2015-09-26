using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wishround.UI.Logic.Entry
{
    public abstract class PropertyEntryBase{
        private bool   _valueSet;
        private object _value;
        private Uri    _url;
        private string _html;


        public bool ValueSet{
            get{
                return _valueSet;
            }
        }


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