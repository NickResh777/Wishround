using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using Wishround.UI.Logic.Entry;
using Wishround.UI.Logic.Parsers;

namespace Wishround.UI.Logic
{
    public class ProductPageDataAnalyzer{
        private readonly Uri     _uri;
        private          string  _htmlContent;
        private readonly bool    _isFoxtrotShop;
  
        private readonly Hashtable _productPropsParsers = new Hashtable();



        public ProductPageDataAnalyzer(string url){
            if (string.IsNullOrEmpty(url))
                 throw new ArgumentException("URL is not defined!", "url");
           

            try{
                // check if the provided URL string 
                // is a valid URI
                _uri = new Uri(url);
            }catch (UriFormatException uriFormatEx){
                string errorMsg = string.Format("Invalid URL format: {0}", url);
                throw new ArgumentException(errorMsg, "url", uriFormatEx);
            }


            _isFoxtrotShop = _uri.AbsoluteUri.IndexOf("foxtrot.com.ua") >= 0;
            InitializeProductProperties();
        }

        private void InitializeProductProperties(){
            _productPropsParsers.Add("Title", new OpenGraphMetaPropertyParser("og:title"));
            _productPropsParsers.Add("Description", new OpenGraphMetaPropertyParser("og:description"));
            _productPropsParsers.Add("Image", new OpenGraphMetaPropertyParser("og:image"));
            _productPropsParsers.Add("Price", _isFoxtrotShop ? (object) new FoxtrotPriceTagPropertyEntry() 
                                                      : new NullPropertyParser<decimal>());
            _productPropsParsers.Add("Currency", _isFoxtrotShop ? (object)new FoxtrotCurrencyPropertyParser() 
                                                      : new NullPropertyParser<string>());
            _productPropsParsers.Add("Code", _isFoxtrotShop ? (object) new FoxtrotProductCodePropertyParser() 
                                                       : new NullPropertyParser<string>());
        }

        public ProductPageDataAnalyzer(string url, string html) : this(url){
           
            _htmlContent = html;
        }

        /// <summary>
        /// Name of the product 
        /// </summary>
        public string ProductTitle{
            get{
                // ensure HTML content is loaded 
                return GetProductProperty<string>("Title");
            }
        }

        public string ProductDescription{
            get {
                // ensure HTML content is loaded
                return GetProductProperty<string>("Description"); 
            }
        }

        public string ProductImageUrl{
            get{
                // ensure HTML provided
                return GetProductProperty<string>("Image");
            }
        }

        public decimal ProductPrice{
            get{
                return GetProductProperty<decimal>("Price");
            }
        }

        public string ProductCurrency{
            get{
                return GetProductProperty<string>("Currency");
            }
        }

        public string ProductCode{
            get{
                return GetProductProperty<string>("Code");
            }
        }

        private TParam GetProductProperty<TParam>(string property){
             EnsureHtmlProvided();
             PropertyParserBase entry = (PropertyParserBase) _productPropsParsers[property];
             if (entry == null)
                 throw new ArgumentException("Property not defined!");
             entry.Html = _htmlContent;
             entry.Uri = _uri;
             return (TParam)entry.Value;
        }


        private static void ValidateResponse(HttpWebResponse response){
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception("Not 200 (OK) return code from server!");
            if (!(response.ContentType).Contains("text/html"))
                throw new Exception("Not valid HTML content response!");
        }

        private void EnsureHtmlProvided(){
            if (_htmlContent == null){

                try{
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(_uri);

         
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    request.Headers["Accept-Language"] = "en-US,en;q=0.8,ru;q=0.6";
                    request.Headers["Cache-Control"] = "no-cache";
                    request.KeepAlive = true;
                    request.UserAgent ="User-Agent:Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.99 Safari/537.36";
                    request.Host = _uri.Host;
              
                  

                    // get an HTTP response
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                    // validate HTTP response
                    ValidateResponse(response);

                    Stream responseStream = response.GetResponseStream();

                    using (StreamReader reader = new StreamReader(responseStream)){

                        _htmlContent = reader.ReadToEnd();
                    }
                }catch (WebException webEx){

                    throw;
                } catch (Exception ex){

                    throw;
                }
            }
        }

    }
}