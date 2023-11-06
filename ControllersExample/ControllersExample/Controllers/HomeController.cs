using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using ControllersExample.Models;

namespace ControllersExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("home")]
        [Route("/")]
        public ContentResult Index()
        {
            //return Content() new ContentResult() { Content = 
            //"Hello from Index", ContentType = "text/plain"
            //};
            //return Content("Hello from Index", "text/plain");

            return Content("<h1>Welcome</h1> <h2>Hello from Index<h2>", "text/html");
        }

        [Route("person")]
        public JsonResult Person()
        {
            Person person = new Person() {Id =  
                Guid.NewGuid(), FirstName = "James", LastName = "Smith", Age = 25};
            //return new JsonResult(person);
            return Json(person);
            //return "{ \"key\": \"value\" }";
        }


        [Route("about")]
        public string About()
        {
            return "Hello from About";
        }

        [Route("file-download")]
        public VirtualFileResult FileFownload()
        {
            //return new VirtualFileResult("/sample.pdf", "application/pdf");
            return File("/sample.pdf", "application/pdf");
        }

        [Route("file-download2")]
        public PhysicalFileResult FileFownload2()
        {
            //return new PhysicalFileResult(@"C:\Users\User\Desktop\sample.pdf", "application/pdf");
            return PhysicalFile(@"C:\Users\User\Desktop\sample.pdf", "application/pdf");
        }

        [Route("file-download3")]
        public FileContentResult FileFownload3()
        {
            byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\User\Desktop\sample.pdf");
            //return new FileContentResult(bytes, "application/pdf");
            return File(bytes, "application/pdf");
        }
    }
}
