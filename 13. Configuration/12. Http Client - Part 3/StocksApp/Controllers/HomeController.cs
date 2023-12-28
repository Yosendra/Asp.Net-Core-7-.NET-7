using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Models;
using StocksApp.Services;

namespace StocksApp.Controllers;

public class HomeController : Controller
{
    private readonly FinnHubService _finnHubService;
    private readonly IOptions<TradingOptions> _tradingOptions;

    public HomeController(FinnHubService finnHubService, IOptions<TradingOptions> tradingOptions)
    {
        _finnHubService = finnHubService;
        _tradingOptions = tradingOptions;         // inject IOptions object
    }

    [Route("/")]
    public async Task<IActionResult> Index()
    {
        string stockSymbol = _tradingOptions.Value.DefaultStockSymbol ?? "MSFT";
        Dictionary<string, object>? responseDictionary = await _finnHubService.GetStockPriceQuote(stockSymbol);

        // Binding the value's of responseDictionary response to the model
        Stock model = new()
        {
            StockSymbol = stockSymbol,

            // we have to use '.ToString()' in each of json element when receiving its value to the model property
            // if not it will be an exception
            CurrentPrice = Convert.ToDouble(responseDictionary["c"].ToString()),    
            HigherPrice = Convert.ToDouble(responseDictionary["h"].ToString()),
            LowestPrice = Convert.ToDouble(responseDictionary["l"].ToString()),
            OpenPrice = Convert.ToDouble(responseDictionary["o"].ToString()),
        };

        return View(model);
    }
}
