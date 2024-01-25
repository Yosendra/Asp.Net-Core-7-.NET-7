using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;

namespace CRUDTests;

public class PersonControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    // we neet to create httpClient object based on our WebApplicationFactory because any request we make through httpClient
    // should be received by our web application
    private readonly HttpClient _httpClient;

    public PersonControllerIntegrationTest(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    #region Index

    [Fact]
    public async Task Index_ReturnView()
    {
        // Arrange

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync("/Person/Index");

        // Assert
        response.Should().BeSuccessful();   // 2xx
        
        string responseBody = await response.Content.ReadAsStringAsync();
        HtmlDocument html = new();
        html.LoadHtml(responseBody);
        var document = html.DocumentNode;   // now we can access DOM element like javascript

        document.QuerySelectorAll("table.person").Should().NotBeNull();
    }

    #endregion
}
