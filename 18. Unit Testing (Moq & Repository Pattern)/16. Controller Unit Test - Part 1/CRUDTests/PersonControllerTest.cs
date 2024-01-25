using AutoFixture;
using CRUDExample.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDTests;

public class PersonControllerTest
{
    private readonly IPersonService _personService;
    private readonly ICountryService _countryService;

    private readonly Mock<IPersonService> _personServiceMock;
    private readonly Mock<ICountryService> _countryServiceMock;

    private readonly IFixture _fixture;

    public PersonControllerTest()
    {
        _fixture = new Fixture();
        _personServiceMock = new Mock<IPersonService>();
        _countryServiceMock = new Mock<ICountryService>();

        _personService = _personServiceMock.Object;
        _countryService = _countryServiceMock.Object;
    }

    [Fact]
    public async Task Index_ShouldReturnIndexViewWithPersonList()
    {
        // Arrange
        List<PersonResponse> personResponseList = _fixture.Create<List<PersonResponse>>();

        _personServiceMock
            .Setup(service => service.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(personResponseList);

        _personServiceMock
            .Setup(service => service.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderEnum>()))
            .ReturnsAsync(personResponseList);

        PersonController personController = new(_personService, _countryService);

        string searchBy = _fixture.Create<string>();
        string? keyword = _fixture.Create<string>();
        string sortBy = _fixture.Create<string>();
        SortOrderEnum sortOrder = _fixture.Create<SortOrderEnum>();

        // Act
        IActionResult result = await personController.Index(searchBy, keyword, sortBy, sortOrder);

        // Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
        viewResult.ViewData.Model.Should().Be(personResponseList);
    }
}
