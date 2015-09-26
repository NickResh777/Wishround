using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using Wishround.UI.Logic.Entry;

namespace Wishround.UI.Logic
{
    public class ProductPageDataAnalyzer{
        private readonly Uri _uri;
        private string _html;
  
        private readonly Hashtable _productProps = new Hashtable();



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


           
            InitializeProductProperties();
        }

        private void InitializeProductProperties(){
            _productProps.Add("Title", new OpenGraphMetaProperty("og:title"));
            _productProps.Add("Description", new OpenGraphMetaProperty("og:description"));
        }

        public ProductPageDataAnalyzer(string url, string html) : this(url){
           
            _html = html;
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

        private TParam GetProductProperty<TParam>(string property){
             EnsureHtmlProvided();
             PropertyEntryBase entry = (PropertyEntryBase) _productProps[property];
             if (entry == null)
                 throw new ArgumentException("Property not defined!");
             entry.Html = _html;
             entry.Uri = _uri;
             return (TParam)entry.Value;
        }


        private static void ValidateResponse(HttpWebResponse response){
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception("Not 200 (OK) return code from server!");
            if (!(response.ContentType ?? string.Empty).Contains("text/html"))
                throw new Exception("Not valid HTML content response!");
        }

        private void EnsureHtmlProvided(){
            if (_html == null){

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

                        _html = reader.ReadToEnd();
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