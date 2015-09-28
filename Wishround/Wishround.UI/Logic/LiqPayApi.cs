using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using Newtonsoft.Json;
using Wishround.UI.Models;

namespace Wishround.UI.Logic
{
    public class LiqPayApi{

        private const int ApiVersion = 3;
        private const string PublicKey = "i41338772090";
        private const string PrivateKey = "elYJFq8psMxca7Xcb55Z2e1N8fAIn1x70TJUaRrI";

        private const string LiqPayApiRoot = "https://www.liqpay.com/api/checkout";

        private const string DescriptionParamFormat = "Chipping in for product: {0}.";

        public static string BuildDataFormParameter(ProductEntity product){
            Hashtable dataParam = new Hashtable();

                dataParam["version"] = ApiVersion;
                dataParam["sandbox"] = 1;
                dataParam["public_key"] = PublicKey;

                dataParam["amount"] = product.Price;
                dataParam["currency"] = product.Currency;
                dataParam["description"] = string.Format(DescriptionParamFormat,
                                               product.Title);
                dataParam["order_id"] = product.Code;
                dataParam["product_url"] = product.ProductUrl;
                dataParam["type"] = "buy";
                dataParam["pay_way"] = "card,liqpay,delayed,invoice,privat24";
                dataParam["language"] = "ru";
                //dataParam["result_url"] = "foxtrot.com.ua";

                // to JSON
                string json =  JsonConvert.SerializeObject(dataParam);
                return ToBase64(json);
        }

        private static string ToBase64(string plainText){
             byte[] bytes = Encoding.UTF8.GetBytes(plainText);
             return Convert.ToBase64String(bytes, Base64FormattingOptions.None);
        }

     

        public static string BuildSignatureFormParameter(string base64_data){
            // {Private key} + <Base64 encoded DATA> + {Private key}
            return ToSHA1Hash(PrivateKey + base64_data + PrivateKey);
        }


        public static string BuildSignatureFormParameter(ProductEntity model){

            return BuildSignatureFormParameter(BuildDataFormParameter(model));
        }

        private static string ToSHA1Hash(string plainText){
           
                using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider()){
                    //byte[] bytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] bytes = Encoding.UTF8.GetBytes(plainText);

                    byte[] hash = sha1.ComputeHash(bytes);
                    return Convert.ToBase64String(hash);
                }
            
        }
    }
}