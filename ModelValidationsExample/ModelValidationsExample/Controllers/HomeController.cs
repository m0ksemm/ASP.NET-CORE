using Microsoft.AspNetCore.Mvc;
using ModelValidationsExample.Models;
using ModelValidationsExample.CustomModelBinders;

namespace ModelValidationsExample.Controllers
{
    public class HomeController : Controller
    {
        //[Bind(nameof(Person.PersonName), nameof(Person.Email), nameof(Person.Password), nameof(Person.ConfirmPassword))]
        //[ModelBinder(BinderType = typeof(PersonModelBinder))]
        [Route("register")]
        public IActionResult Index(Person person, [FromHeader(Name = "User-Agent")]string UserAgent)
        {
            if (!ModelState.IsValid) 
            {
                string errors = string.Join("\n", 
                ModelState.Values.SelectMany(value =>
                value.Errors).Select(err => err.ErrorMessage));
                
                return BadRequest(errors);
            }


            return Content($"{person}, {UserAgent}");
        }
    }
}
