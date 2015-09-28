using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace Wishround.UI.Models
{
    public sealed class ProductEntity
    {
        /// <summary>
        /// ID of product in the database 
        /// </summary>
        public Guid ProductId { get; set; }


        /// <summary>
        /// URL of the product page on the store
        /// </summary>
        public string ProductUrl { get; set; }


        /// <summary>
        /// Product code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Title of the product
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of the product
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Price of product
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// URL of the product image
        /// </summary>
        public string ImageUrl { get; set; }
    }
}