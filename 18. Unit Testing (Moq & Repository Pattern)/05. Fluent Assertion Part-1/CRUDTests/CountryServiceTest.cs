using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests;

public class CountryServiceTest
{
    #region Fields
    private readonly ICountryService _countryService;
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
    }
    #endregion

    #region AddCountry()
    [Fact]
    public async Task AddCountry_RequestModelNull_ThrowsArgumentNullException()
    {
        // Arrange
        CountryAddRequest request = null;

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>         // notice this
        {
            // Act
            await _countryService.AddCountry(request);
        });
    }

    [Fact]
    public async Task AddCountry_CountryNameNull_ThrowsArgumentException()
    {
        // Arrange
        CountryAddRequest request = new() { Name = null };

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => 
        {
            // Act
            await _countryService.AddCountry(request);
        });
    }

    [Fact]
    public async Task AddCountry_CountryNameDuplicate_ThrowsArgumentException()
    {
        // Arrange
        string countryName = "USA";
        CountryAddRequest request = new() { Name = countryName, };

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => 
        {
            // Act
            await _countryService.AddCountry(request);
            await _countryService.AddCountry(request);
        });
    }

    [Fact]
    public async Task AddCountry_Success_ReturnCountryResponseWithId()
    {
        // Arrange
        string countryName = "Japan";
        CountryAddRequest request = new() { Name = countryName, };

        // Act
        CountryResponse response = await _countryService.AddCountry(request);

        // Assert
        Assert.True(response.Id != Guid.Empty);
    }
    #endregion

    #region GetAllCountries()
    [Fact]
    public async Task GetAllCountries_CountriesEmpty_ReturnEmptyList()
    {
        // Act
        List<CountryResponse> actualResult = await _countryService.GetAllCountries();

        // Assert
        Assert.Empty(actualResult);
    }

    [Fact]
    public async Task GetAllCountries_CountriesExist_ReturnSavedCountryList()
    {
        // Arrange
        List<CountryAddRequest> countryAddRequestList = new()
        {
            new CountryAddRequest { Name = "America" },
            new CountryAddRequest { Name = "Japan" },
            new CountryAddRequest { Name = "Rusia" },
        };

        List<CountryResponse> expectedSavedCountryList = new();
        foreach (var countryAddRequest in countryAddRequestList)
        {
            CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);
            expectedSavedCountryList.Add(countryResponse);
        }

        // Act
        List<CountryResponse> result = await _countryService.GetAllCountries();

        // Assert
        foreach (var country in result)
            Assert.Contains(country, expectedSavedCountryList);
    }
    #endregion

    #region GetCountryById()
    [Fact]
    public async Task GetCountryById_IdNull_ReturnNull()
    {
        // Arrange
        Guid? id = null;
        
        // Act
        CountryResponse? result = await _countryService.GetCountryById(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetCountryById_Exist_ReturnCountryResponse()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new() { Name = "China" };
        CountryResponse savedCountry = await _countryService.AddCountry(countryAddRequest);
        Guid? savedCountryId = savedCountry.Id;

        // Act
        CountryResponse? result = await _countryService.GetCountryById(savedCountryId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(savedCountry, result);
    }

    [Fact]
    public async Task GetCountryById_NotExist_ReturnNull()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new() { Name = "China" };
        await _countryService.AddCountry(countryAddRequest);
        Guid unknownId = Guid.Parse("355558CB-285F-4272-AA72-F5ECFF18DD88");

        // Act
        CountryResponse? result = await _countryService.GetCountryById(unknownId);

        // Assert
        Assert.Null(result);
    }
    #endregion
}
