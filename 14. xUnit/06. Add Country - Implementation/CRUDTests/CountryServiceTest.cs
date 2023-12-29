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
    // When CountryAddRequest is null, it should throw ArgumentNullException
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
    // When Countryname is null, it should throw ArgumentException
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
    // When CountryName is duplicate, it should throw ArgumentException
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
    // When supplied with proper CountryName, it should insert / add the country to the existing list of countries
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
}
