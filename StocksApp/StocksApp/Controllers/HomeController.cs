using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Models;
using StocksApp.Services;

namespace StocksApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly FinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        public HomeController(FinnhubService finnhubService, IOptions<TradingOptions> tradingOptions) 
        {
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions;
        }
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if (_tradingOptions.Value.DefaultStockSymbol == null)
            {
                _tradingOptions.Value.DefaultStockSymbol = "MSFT";
            }

            Dictionary<string, object>? responseDictionary = await _finnhubService.GetStockPriceQuote(_tradingOptions.Value.DefaultStockSymbol);

            Stock stock = new Stock()
            {
                StockSymbol = _tradingOptions.Value.DefaultStockSymbol,
                CurrentPrice = double.Parse(responseDictionary["c"].ToString(), System.Globalization.CultureInfo.InvariantCulture),
                HighestPrice = double.Parse(responseDictionary["h"].ToString(), System.Globalization.CultureInfo.InvariantCulture),
                LowestPrice = double.Parse(responseDictionary["l"].ToString(), System.Globalization.CultureInfo.InvariantCulture),
                OpenPrice = double.Parse(responseDictionary["o"].ToString(), System.Globalization.CultureInfo.InvariantCulture)
            };

            return View(stock);
        }
    }
}
