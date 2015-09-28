using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Wishround.UI.Models;

namespace Wishround.UI.Logic
{
    public class ProductsStorage{
        private static readonly ProductsStorage SingletonInstance = new ProductsStorage();

        private static string ConnectionString;

        private const string InsertProductSqlScript = @"insert into Wishes(PageUrl, 
                                                                           ProductCode,
                                                                           ProductTitle, 
                                                                           ProductDescription,
                                                                           ProductImageUrl,
                                                                           ProductPrice, 
                                                                           ProductCurrency)
                                                        select @PageUrl, 
                                                               @Code,
                                                               @Title,
                                                               @Description,
                                                               @ImageUrl,
                                                               @Price,
                                                               @Currency";


        // hidden constructor for Singleton instance
        private ProductsStorage(){

            var connCS = ConfigurationManager.ConnectionStrings["DefaultConnection"];
            ConnectionString = connCS.ConnectionString;
        }

        public static ProductsStorage Instance{
            get{
                return SingletonInstance;
            }
        }


        public void SaveProduct(ProductEntity product){
            if (product == null)
                throw new ArgumentNullException("product");

            using (var conn = new SqlConnection(ConnectionString)){
                using (var insertCmd = new SqlCommand(InsertProductSqlScript, conn)){
                    // open connection to the database

                    insertCmd.Parameters.Add("@PageUrl", SqlDbType.NVarChar).Value = product.ProductUrl;
                    insertCmd.Parameters.Add("@Code", SqlDbType.NVarChar).Value = (object) product.Code ?? DBNull.Value;
                    insertCmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = product.Title;
                    insertCmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = product.Description;
                    insertCmd.Parameters.Add("@ImageUrl", SqlDbType.NVarChar).Value = product.ImageUrl;
                    insertCmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = product.Price;
                    insertCmd.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = (object) product.Currency ?? DBNull.Value;

                    insertCmd.Connection.Open();


                    // execute the SQL insert query
                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        public ProductEntity[] GetProducts(){
            
            return null;
        }
    }
}