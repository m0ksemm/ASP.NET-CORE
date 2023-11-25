using Microsoft.AspNetCore.Mvc;

namespace LayoutViewsExample.Controllers
{
    public class ProductsController : Controller
    {
        [Route("products")]
        public IActionResult Index()
        {
            return View();
        }

        //Url: /search-products/1
        [Route("search-products/{ProductId?}")] 
        public IActionResult Search(int? ProductId)
        {
            ViewBag.ProductID = ProductId;
            return View();
        }

        [Route("order-product")]
        public IActionResult Order()
        {
            return View();
        }
    }
}
