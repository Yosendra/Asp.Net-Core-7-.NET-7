using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace CRUDTests;

public class PersonServiceTest
{
    #region Fields
    private readonly IPersonService _personService;
    #endregion

    #region Constructor
    public PersonServiceTest()
    {
        _personService = new PersonService();
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
}
