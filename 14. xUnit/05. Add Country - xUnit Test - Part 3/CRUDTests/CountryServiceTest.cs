using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests;

public class CountryServiceTest
{
    private readonly ICountryService _countryService;

    public CountryServiceTest()
    {
        _countryService = new CountryService();
    }

    // When CountryAddRequest is null, it should throw ArgumentNullException
    [Fact]
    public void AddCountry_NullRequest()
    {
        #region Arrange
        CountryAddRequest request = null;
        #endregion

        #region Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            #region Act
            _countryService.AddCountry(request);
            #endregion
        });
        #endregion
    }

    // When Countryname is null, it should throw ArgumentException
    [Fact]
    public void AddCountry_CountryNameIsNull()
    {
        #region Arrange
        CountryAddRequest request = new()
        {
            CountryName = null,
        };
        #endregion

        #region Assert
        Assert.Throws<ArgumentException>(() =>
        {
            #region Act
            _countryService.AddCountry(request);
            #endregion
        });
        #endregion
    }

    // When CountryName is duplicate, it should throw ArgumentException
    [Fact]
    public void AddCountry_CountryNameIsDuplicate()
    {
        #region Arrange
        string countryName = "USA";
        CountryAddRequest request1 = new() { CountryName = countryName, };
        CountryAddRequest request2 = new() { CountryName = countryName, };
        #endregion

        #region Assert
        Assert.Throws<ArgumentException>(() =>
        {
            #region Act
            _countryService.AddCountry(request1);
            _countryService.AddCountry(request2);
            #endregion
        });
        #endregion
    }

    // When supplied property CountryName, it should insert / add the country to the existing list of countries
    [Fact]
    public void AddCountry_ProperCountryDetails()
    {
        #region Arrange
        string countryName = "Japan";
        CountryAddRequest request = new() { CountryName = countryName, };
        #endregion

        #region Act
        CountryResponse response = _countryService.AddCountry(request);
        #endregion

        #region Assert
        Assert.True(response.CountryId != Guid.Empty);
        #endregion
    }
}
