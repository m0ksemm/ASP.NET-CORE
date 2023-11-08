using Microsoft.AspNetCore.Mvc;
using System;
using BankApplication.Models;

using System.Text.RegularExpressions;
using System.Collections.Generic;


namespace BankApplication.Controllers
{
    public class MainController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return Content("Wellcome to the Best Bank!");
        }

        [Route("account-details")]      
        public JsonResult Details()
        {
            Details first = new Details() { accountNumber = 1001, accountHolderName = "Example Name\n", currentBalance = 5000 };
            return Json(first);
        }
        [Route("account-statement")]
        public VirtualFileResult Statement()
        {
            return File("/Sample.pdf", "application/pdf");
        }

        [Route("get-current-balance/{accountNumber}")]
        public IActionResult currentBalance()
        {
            int numb = Convert.ToInt32(Request.RouteValues["accountNumber"]);
            if (numb != 1001)
            {
                return BadRequest($"{numb}:Account Number should be 1001");
            }
            else
            {
                return Content("5000", "text/plain");
            }
        }

        [Route("get-current-balance")]
        public IActionResult error()
        {
            return NotFound("Account Number should be supplied");
        }

    }

}