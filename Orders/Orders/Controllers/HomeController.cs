using Microsoft.AspNetCore.Mvc;
using Orders.Models;

namespace Orders.Controllers
{
    public class HomeController : Controller
    {
        [Route("/order")]
        public IActionResult Index(Order order)
        {
            if (!ModelState.IsValid)
            {
                //get error messages from model state
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            Random rnd = new Random();
            order.OrderNo = rnd.Next(1, 99999);
            return Json(new { order.OrderNo});
        }
    }
}