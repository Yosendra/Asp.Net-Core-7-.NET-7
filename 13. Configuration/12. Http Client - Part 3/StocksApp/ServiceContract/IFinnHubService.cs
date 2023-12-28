namespace StocksApp.ServiceContract;

public interface IFinnHubService
{
    Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
}
