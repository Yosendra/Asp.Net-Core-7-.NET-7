using System.Text.Json;
using StocksApp.ServiceContract;

namespace StocksApp.Services;

public class FinnHubService : IFinnHubService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;     // Inject IConfiguration object

    // Inject HttpClientFactory object in constructor
    public FinnHubService(IHttpClientFactory httpClientFactory,
                          IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
    {
        // create an instance of HttpClient, automatically dispose
        using HttpClient httpClient = _httpClientFactory.CreateClient();

        // Get the api key from newest config read, in this case we use user-secrets manager,
        // but for fallback, we also provide the key in development config setting
        string apiKey = _configuration["FinnHubToken"]; 
        HttpRequestMessage httpRequestMessage = new()
        {
            // look at https://finnhub.io/docs/api/quote documentation
            // Is it safe to store this api key in source code? NO
            // this api key is hard-coded. we cannot dynamically change it
            // we can use appsetting.json, or user-secrets manager to store it
            RequestUri = new Uri($"https://finnhub.io/api/v1/quote" +
                                 $"?symbol={stockSymbol}" +
                                 $"&token={apiKey}"), // this is the webservice url
            Method = HttpMethod.Get,
        };
        HttpResponseMessage responseMessage = await httpClient.SendAsync(httpRequestMessage);

        Stream stream = await responseMessage.Content.ReadAsStreamAsync();
        using StreamReader streamReader = new(stream);

        string content = await streamReader.ReadToEndAsync();
        var responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(content);

        // Sometime we might get error from FinnHub webservice
        if (responseDictionary is null)
            throw new InvalidOperationException("No response from FinnHub server");

        if (responseDictionary.ContainsKey("error"))
        {
            string? errorMessage = Convert.ToString(responseDictionary["error"]);
            throw new InvalidOperationException("No response from FinnHub server");
        }

        return responseDictionary;
    }
}
