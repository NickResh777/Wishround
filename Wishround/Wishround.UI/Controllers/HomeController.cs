using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wishround.UI.Logic;

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

            string title = analyzer.ProductTitle;
            string description = analyzer.ProductDescription;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}