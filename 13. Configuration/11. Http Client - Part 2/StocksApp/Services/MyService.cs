namespace StocksApp.Services;

public class MyService
{
    private readonly IHttpClientFactory _httpClientFactory;

    // Inject HttpClientFactory object in constructor
    public MyService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task method()
    {
        // create an instance of HttpClient, automatically dispose
        using HttpClient httpClient = _httpClientFactory.CreateClient();

        HttpRequestMessage httpRequestMessage = new()
        {
            // look at https://finnhub.io/docs/api/quote documentation
            RequestUri = new Uri("https://finnhub.io/api/v1/quote?symbol=AAPL&token=cm5shnpr01qjc6l4nqr0cm5shnpr01qjc6l4nqrg"), // this is the webservice url
            Method = HttpMethod.Get,
        };
        HttpResponseMessage responseMessage = await httpClient.SendAsync(httpRequestMessage);

        Stream stream = await responseMessage.Content.ReadAsStreamAsync();
        using StreamReader streamReader = new(stream);

        string content = await streamReader.ReadToEndAsync();
    }
}
