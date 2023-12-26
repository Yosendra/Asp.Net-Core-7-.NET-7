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
            RequestUri = new Uri("url"),
            Method = HttpMethod.Get,
        };
        HttpResponseMessage responseMessage = await httpClient.SendAsync(httpRequestMessage);
    }
}
