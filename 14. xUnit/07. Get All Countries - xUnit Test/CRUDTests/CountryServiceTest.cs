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
    // The list of countries should be empty by default (before adding any countries)
    public void GetAllCountries_EmptyList()
    {
        // Act
        List<CountryResponse> actualResult = _countryService.GetAllCountries();

        // Assert
        Assert.Empty(actualResult);
    }

    [Fact]
    public void GetAllCountries_AddFewCountries()
    {
        // Arrange
        List<CountryAddRequest> countryAddRequestList = new()
        {
            new CountryAddRequest { CountryName = "America" },
            new CountryAddRequest { CountryName = "Japan" },
            new CountryAddRequest { CountryName = "Rusia" },
        };

        List<CountryResponse> expectedCountryResponseList = new();
        foreach (var countryAddRequest in countryAddRequestList)
        {
            expectedCountryResponseList.Add(_countryService.AddCountry(countryAddRequest));
        }

        // Act
        List<CountryResponse> actualCountryResponseList = _countryService.GetAllCountries();

        // Assert
        foreach (var actualCountryResponse in actualCountryResponseList)
        {
            Assert.Contains(actualCountryResponse, expectedCountryResponseList);
        }
    }
    #endregion
}
