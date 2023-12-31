using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace CRUDTests;

public class PersonServiceTest
{
    #region Fields
    private readonly IPersonService _personService;
    private readonly ICountryService _countryService;
    #endregion

    #region Constructor
    public PersonServiceTest()
    {
        _personService = new PersonService();
        _countryService = new CountryService();
    }
    #endregion

    #region AddPerson()
    [Fact]
    public void AddPerson_RequestModelNull_ThrowsArgumentNullException()
    {
        // Arrange
        PersonAddRequest? requestModel = null;

        // Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            // Act
            _personService.AddPerson(requestModel);
        });
    }

    [Fact]
    public void AddPerson_NameNull_ThrowsArgumentException()
    {
        // Arrange
        PersonAddRequest? requestModel = new()
        {
            Name = null,
        };

        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            // Act
            _personService.AddPerson(requestModel);
        });
    }

    [Fact]
    public void AddPerson_Success_ReturnPersonResponseWithId()
    {
        // Arrange
        PersonAddRequest? requestModel = new()
        {
            Name = "Jack",
            Address = "21st Jump Street",
            Email = "Jack@mailinator.com",
            Gender = GenderOption.Male,
            DateOfBirth = DateTime.Parse("1995-01-01"),
            ReceiveNewsLetters = false,
            CountryId = Guid.NewGuid(),
        };

        // Act
        PersonResponse response = _personService.AddPerson(requestModel);

        // Assert
        Assert.True(response.Id != Guid.Empty);
    }
    #endregion

    #region GetPersonById
    [Fact]
    public void GetPersonById_IdNull_ReturnNull()
    {
        // Arrange
        Guid? id = null;

        // Act
        PersonResponse? response = _personService.GetPersonById(id);

        // Assert
        Assert.Null(response);
    }

    [Fact]
    public void GetPersonById_Exist_ReturnCorrectPersonResponse()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new(){ Name = "Canada", };
        CountryResponse countryResponse = _countryService.AddCountry(countryAddRequest);

        PersonAddRequest? personAddRequest = new()
        {
            Name = "Jack",
            Address = "21st Jump Street",
            Email = "Jack@mailinator.com",
            Gender = GenderOption.Male,
            DateOfBirth = DateTime.Parse("1995-01-01"),
            ReceiveNewsLetters = false,
            CountryId = countryResponse.Id,
        };
        PersonResponse expectedPersonResponse = _personService.AddPerson(personAddRequest);
        Guid? addedPersonId = expectedPersonResponse.Id;

        // Act
        PersonResponse? actualPersonResponse = _personService.GetPersonById(addedPersonId);

        // Assert
        Assert.Equal(expectedPersonResponse, actualPersonResponse);
    }
    #endregion
}
