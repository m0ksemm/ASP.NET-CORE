using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Xml.Linq;
using System;
using Weather.Models;

namespace Weather.Controllers
{
    public class HomeController : Controller
    {
        [Route("home")]
        [Route("/")]
        public IActionResult Index()
        {
            List<WeatherInfo> WeatherItems = new List<WeatherInfo>()
            {
                new WeatherInfo() { CityUniqueCode = "LDN", CityName = "London", DateAndTime = DateTime.Parse("2030-01-01 8:00"), TemperatureFahrenheit = 33 },
                new WeatherInfo() { CityUniqueCode = "NYC", CityName = "New York", DateAndTime = DateTime.Parse("2030-01-01 3:00"), TemperatureFahrenheit = 60 },
                new WeatherInfo() { CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = DateTime.Parse("2030-01-01 9:00"), TemperatureFahrenheit = 82 }
            };

            return View("Index", WeatherItems);
        }

        [Route("weather/{code}")]
        public IActionResult Details(string? code)
        {
            if (code == null) 
                return Content("City Unique Code can't be null"); 
            List<WeatherInfo> WeatherItems = new List<WeatherInfo>()
            {
                new WeatherInfo() { CityUniqueCode = "LDN", CityName = "London", DateAndTime = DateTime.Parse("2030-01-01 8:00"), TemperatureFahrenheit = 33 },
                new WeatherInfo() { CityUniqueCode = "NYC", CityName = "New York", DateAndTime = DateTime.Parse("2030-01-01 3:00"), TemperatureFahrenheit = 60 },
                new WeatherInfo() { CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = DateTime.Parse("2030-01-01 9:00"), TemperatureFahrenheit = 82 }
            };

            WeatherInfo? matchingCity = WeatherItems.Where(temp => temp.CityUniqueCode == code).FirstOrDefault();
            if (matchingCity != null)
                return View(matchingCity);
            else return View("InvalidCityCode");
        }
    }
}
