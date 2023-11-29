using Microsoft.AspNetCore.Mvc;
using Weather.Models;

namespace Weather.ViewComponents
{
    public class CityViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(WeatherInfo weatherinfo)
        {

            return View("Sample", weatherinfo); //invoked a partial view Views/Shared/Components/Grid/Default.cshtml
        }
    }
}
