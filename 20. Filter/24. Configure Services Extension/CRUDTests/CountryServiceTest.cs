using AutoFixture;
using Entities;
using FluentAssertions;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests;

public class CountryServiceTest
{
    #region Fields
    private readonly ICountryService _countryService;

    private readonly ICountryRepository _countryRepository;
    private readonly Mock<ICountryRepository> _countryRepositoryMock;

    private readonly IFixture _fixture;
    #endregion

    #region Constructor
    public CountryServiceTest()
    {
        _countryRepositoryMock = new Mock<ICountryRepository>();
        _countryRepository = _countryRepositoryMock.Object;

        _countryService = new CountryService(_countryRepository);
        _fixture = new Fixture();
    }
    #endregion

    #region AddCountry
    [Fact]
    public async Task AddCountry_RequestModelNull_ThrowsArgumentNullException()
    {
        // Arrange
        CountryAddRequest? request = null;

        // Act
        Func<Task> action = async () => await _countryService.AddCountry(request);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddCountry_CountryNameNull_ThrowsArgumentException()
    {
        // Arrange
        var request = _fixture.Build<CountryAddRequest>()
            .With(p => p.Name, null as string)
            .Create();

        // Act
        Func<Task> action = async () => await _countryService.AddCountry(request);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddCountry_CountryNameDuplicate_ThrowsArgumentException()
    {
        // Arrange
        var request = _fixture.Create<CountryAddRequest>();
        var country = request.ToCountry();

        _countryRepositoryMock
            .Setup(repo => repo.GetCountryByName(It.IsAny<string>()))
            .ReturnsAsync(country);

        // Act
        Func<Task> action = async () => await _countryService.AddCountry(request);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddCountry_Success_ReturnCountryResponseWithId()
    {
        // Arrange
        var request = _fixture.Create<CountryAddRequest>();

        // Act
        CountryResponse response = await _countryService.AddCountry(request);

        // Assert
        response.Id.Should().NotBeEmpty();
    }
    #endregion

    #region GetAllCountries
    [Fact]
    public async Task GetAllCountries_CountriesEmpty_ReturnEmptyList()
    {
        // Arrange
        List<Country> countries = new();

        _countryRepositoryMock
            .Setup(repo => repo.GetAllCountries())
            .ReturnsAsync(countries);

        // Act
        List<CountryResponse> actualResult = await _countryService.GetAllCountries();

        // Assert
        actualResult.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllCountries_CountriesExist_ReturnSavedCountryList()
    {
        // Arrange
        List<Country> countries = new()
        {
            _fixture.Build<Country>().With(c => c.Persons, null as List<Person>).Create(),
            _fixture.Build<Country>().With(c => c.Persons, null as List<Person>).Create(),
            _fixture.Build<Country>().With(c => c.Persons, null as List<Person>).Create(),
        };

        _countryRepositoryMock
            .Setup(repo => repo.GetAllCountries())
            .ReturnsAsync(countries);

        List<CountryResponse> expected = countries.ConvertAll(c => c.ToCountryResponse());

        // Act
        List<CountryResponse> result = await _countryService.GetAllCountries();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
    #endregion

    #region GetCountryById
    [Fact]
    public async Task GetCountryById_IdNull_ReturnNull()
    {
        // Arrange
        Guid? id = null;
        
        // Act
        CountryResponse? result = await _countryService.GetCountryById(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCountryById_Exist_ReturnCountryResponse()
    {
        // Arrange
        var country = _fixture.Build<Country>().With(c => c.Persons, null as List<Person>).Create();

        _countryRepositoryMock
            .Setup(repo => repo.GetCountryById(It.IsAny<Guid>()))
            .ReturnsAsync(country);

        Guid? savedCountryId = country.Id;
        var expected = country.ToCountryResponse();

        // Act
        CountryResponse? result = await _countryService.GetCountryById(savedCountryId);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expected);
    }

    [Fact]
    public async Task GetCountryById_NotExist_ReturnNull()
    {
        // Arrange
        var countryAddRequest = _fixture.Create<CountryAddRequest>();
        await _countryService.AddCountry(countryAddRequest);

        Guid unknownId = Guid.Parse("355558CB-285F-4272-AA72-F5ECFF18DD88");

        // Act
        CountryResponse? result = await _countryService.GetCountryById(unknownId);

        // Assert
        result.Should().BeNull();
    }
    #endregion
}
