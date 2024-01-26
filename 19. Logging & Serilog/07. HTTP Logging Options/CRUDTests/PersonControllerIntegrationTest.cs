using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;

namespace CRUDTests;

public class PersonControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
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
        var document = html.DocumentNode;

        document.QuerySelectorAll("table.person").Should().NotBeNull();
    }

    #endregion
}
