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
        _countryService = new CountryService();
    }
    #endregion

    #region AddCountry()
    [Fact]
    public void AddCountry_RequestModelNull_ThrowsArgumentNullException()
    {
        // Arrange
        CountryAddRequest request = null;

        // Assert
        Assert.Throws<ArgumentNullException>(() => 
        {
            // Act
            _countryService.AddCountry(request);
        });
    }

    [Fact]
    public void AddCountry_CountryNameNull_ThrowsArgumentException()
    {
        // Arrange
        CountryAddRequest request = new() { CountryName = null };

        // Assert
        Assert.Throws<ArgumentException>(() => 
        {
            // Act
            _countryService.AddCountry(request);
        });
    }

    [Fact]
    public void AddCountry_CountryNameDuplicate_ThrowsArgumentException()
    {
        // Arrange
        string countryName = "USA";
        CountryAddRequest request = new() { CountryName = countryName, };

        // Assert
        Assert.Throws<ArgumentException>(() => 
        {
            // Act
            _countryService.AddCountry(request);
            _countryService.AddCountry(request);
        });
    }

    [Fact]
    public void AddCountry_Success_ReturnCountryResponseWithId()
    {
        // Arrange
        string countryName = "Japan";
        CountryAddRequest request = new() { CountryName = countryName, };

        // Act
        CountryResponse response = _countryService.AddCountry(request);

        // Assert
        Assert.True(response.CountryId != Guid.Empty);
    }
    #endregion

    #region GetAllCountries()
    [Fact]
    public void GetAllCountries_CountriesEmpty_ReturnEmptyList()
    {
        // Act
        List<CountryResponse> actualResult = _countryService.GetAllCountries();

        // Assert
        Assert.Empty(actualResult);
    }

    [Fact]
    public void GetAllCountries_CountriesExist_ReturnSavedCountryList()
    {
        // Arrange
        List<CountryAddRequest> countryAddRequestList = new()
        {
            new CountryAddRequest { CountryName = "America" },
            new CountryAddRequest { CountryName = "Japan" },
            new CountryAddRequest { CountryName = "Rusia" },
        };

        List<CountryResponse> expectedSavedCountryList = new();
        foreach (var countryAddRequest in countryAddRequestList)
        {
            CountryResponse countryResponse = _countryService.AddCountry(countryAddRequest);
            expectedSavedCountryList.Add(countryResponse);
        }

        // Act
        List<CountryResponse> result = _countryService.GetAllCountries();

        // Assert
        foreach (var country in result)
            Assert.Contains(country, expectedSavedCountryList);
    }
    #endregion

    #region GetCountryById()
    [Fact]
    public void GetCountryById_IdNull_ReturnNull()
    {
        // Arrange
        Guid? id = null;
        
        // Act
        CountryResponse? result = _countryService.GetCountryById(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetCountryById_Exist_ReturnCountryResponse()
    {
        int? a = 10;
        Nullable<int> b = 1;

        // Arrange
        CountryAddRequest countryAddRequest = new() { CountryName = "China" };
        CountryResponse savedCountry = _countryService.AddCountry(countryAddRequest);
        Guid? savedCountryId = savedCountry.CountryId;

        // Act
        CountryResponse? result = _countryService.GetCountryById(savedCountryId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(savedCountry, result);
    }

    [Fact]
    public void GetCountryById_NotExist_ReturnNull()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new() { CountryName = "China" };
        _countryService.AddCountry(countryAddRequest);
        Guid unknownId = Guid.Parse("355558CB-285F-4272-AA72-F5ECFF18DD88");

        // Act
        CountryResponse? result = _countryService.GetCountryById(unknownId);

        // Assert
        Assert.Null(result);
    }
    #endregion
}
