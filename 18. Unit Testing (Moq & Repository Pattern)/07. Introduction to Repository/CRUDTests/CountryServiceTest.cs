using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests;

public class CountryServiceTest
{
    #region Fields
    private readonly ICountryService _countryService;
    private readonly IFixture _fixture;
    #endregion

    #region Constructor
    public CountryServiceTest()
    {
        var initialData = new List<Country>();

        // Mock the DbContext
        var dbContextOption = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        DbContextMock<ApplicationDbContext> dbContextMock = new(dbContextOption);

        // Mock the DbSet functionality and load the initial data for countries
        dbContextMock.CreateDbSetMock(t => t.Countries, initialData);

        ApplicationDbContext dbContext = dbContextMock.Object;

        _countryService = new CountryService(dbContext);
        _fixture = new Fixture();
    }
    #endregion

    #region AddCountry()
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
        var request = 
            _fixture.Build<CountryAddRequest>()
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
        await _countryService.AddCountry(request);

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
        CountryResponse response = 
            await _countryService.AddCountry(request);

        // Assert
        response.Id.Should().NotBeEmpty();
    }
    #endregion

    #region GetAllCountries()
    [Fact]
    public async Task GetAllCountries_CountriesEmpty_ReturnEmptyList()
    {
        // Act
        List<CountryResponse> actualResult = 
            await _countryService.GetAllCountries();

        // Assert
        actualResult.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllCountries_CountriesExist_ReturnSavedCountryList()
    {
        // Arrange
        List<CountryAddRequest> countryAddRequestList = new()
        {
            _fixture.Create<CountryAddRequest>(),
            _fixture.Create<CountryAddRequest>(),
            _fixture.Create<CountryAddRequest>(),
        };

        List<CountryResponse> expectedSavedCountryList = new();
        foreach (var countryAddRequest in countryAddRequestList)
        {
            CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);
            expectedSavedCountryList.Add(countryResponse);
        }

        // Act
        List<CountryResponse> result = 
            await _countryService.GetAllCountries();

        // Assert
        result.Should().BeEquivalentTo(expectedSavedCountryList);
    }
    #endregion

    #region GetCountryById()
    [Fact]
    public async Task GetCountryById_IdNull_ReturnNull()
    {
        // Arrange
        Guid? id = null;
        
        // Act
        CountryResponse? result = 
            await _countryService.GetCountryById(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCountryById_Exist_ReturnCountryResponse()
    {
        // Arrange
        var countryAddRequest = _fixture.Create<CountryAddRequest>();
        CountryResponse savedCountry = await _countryService.AddCountry(countryAddRequest);
        Guid? savedCountryId = savedCountry.Id;

        // Act
        CountryResponse? result = 
            await _countryService.GetCountryById(savedCountryId);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(savedCountry);
    }

    [Fact]
    public async Task GetCountryById_NotExist_ReturnNull()
    {
        // Arrange
        var countryAddRequest = _fixture.Create<CountryAddRequest>();
        await _countryService.AddCountry(countryAddRequest);

        Guid unknownId = Guid.Parse("355558CB-285F-4272-AA72-F5ECFF18DD88");

        // Act
        CountryResponse? result = 
            await _countryService.GetCountryById(unknownId);

        // Assert
        result.Should().BeNull();
    }
    #endregion
}
