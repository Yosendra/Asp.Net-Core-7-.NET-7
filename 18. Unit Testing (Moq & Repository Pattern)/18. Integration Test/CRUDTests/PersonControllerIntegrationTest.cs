using FluentAssertions;

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
        response.Should().BeSuccessful();
    }

    #endregion


}
