using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wishround.UI.Logic;
using Wishround.UI.Models;

namespace Wishround.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Wish(string url)
        {
            ProductPageDataAnalyzer analyzer = new ProductPageDataAnalyzer(url);


            ProductEntity product = new ProductEntity{
                ProductUrl = url,
                Code = analyzer.ProductCode,
                Title = analyzer.ProductTitle,
                Description = analyzer.ProductDescription,
                ImageUrl = analyzer.ProductImageUrl,
                Currency = analyzer.ProductCurrency,
                Price = analyzer.ProductPrice
            };


            return View(product);
        }

        [HttpPost]
        public ActionResult CommitWish(ProductEntity product){
            
            ProductsStorage.Instance.SaveProduct(product);
            return View("ShareWish", product);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}