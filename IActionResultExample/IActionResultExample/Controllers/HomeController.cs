using Microsoft.AspNetCore.Mvc;
using IActionResultExample.Models;
using System;

namespace IActionResultExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("bookstore/{bookid?}/{isloggedin?}")]
        public IActionResult Index([FromQuery]int? bookid, [FromRoute]bool? isloggedin, Book book)
        {
            //Book id should be applied
            if (bookid.HasValue == false)
            {
                return Content("Book id is not supplied or emty");
            }
 
            if (bookid <= 0) 
            {
                return NotFound("Book id can't be less than or equal to zero");
            }
            if (bookid > 1000)
            {
                return NotFound("Book id can't be greater than 1000");
            }

            //isloggedin should be true
            if (isloggedin == false) 
            {
                return StatusCode(401);
            }

            return Content($"Book id: {bookid}, Book: {book}", "text/plain");
        }
    }
}