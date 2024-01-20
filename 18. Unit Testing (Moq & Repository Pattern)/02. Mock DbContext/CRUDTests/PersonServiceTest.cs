using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
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
        var countryInitialData = new List<Country>();
        var personInitialData = new List<Person>();

        // Mock the DbContext
        var dbContextOption = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        DbContextMock<ApplicationDbContext> dbContextMock = new(dbContextOption);

        // Mock the DbSet functionality and load the initial data for countries and person
        dbContextMock.CreateDbSetMock(t => t.Countries, countryInitialData);
        dbContextMock.CreateDbSetMock(t => t.Persons, personInitialData);

        ApplicationDbContext dbContext = dbContextMock.Object;

        _countryService = new CountryService(dbContext);
        _personService = new PersonService(dbContext);
    }
    #endregion

    #region AddPerson
    [Fact]
    public async Task AddPerson_RequestModelNull_ThrowsArgumentNullException()
    {
        // Arrange
        PersonAddRequest? requestModel = null;

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            // Act
            await _personService.AddPerson(requestModel);
        });
    }

    [Fact]
    public async Task AddPerson_NameNull_ThrowsArgumentException()
    {
        // Arrange
        PersonAddRequest? requestModel = new()
        {
            Name = null,
        };

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _personService.AddPerson(requestModel);
        });
    }

    [Fact]
    public async Task AddPerson_Success_ReturnPersonResponseWithId()
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
        PersonResponse response = await _personService.AddPerson(requestModel);

        // Assert
        Assert.True(response.Id != Guid.Empty);
    }
    #endregion

    #region GetPersonById
    [Fact]
    public async Task GetPersonById_IdNull_ReturnNull()
    {
        // Arrange
        Guid? id = null;

        // Act
        PersonResponse? response = await _personService.GetPersonById(id);

        // Assert
        Assert.Null(response);
    }

    [Fact]
    public async Task GetPersonById_Exist_ReturnCorrectPersonResponse()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new(){ Name = "Canada", };
        CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);

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
        PersonResponse expectedPersonResponse = await _personService.AddPerson(personAddRequest);
        Guid? addedPersonId = expectedPersonResponse.Id;

        // Act
        PersonResponse? actualPersonResponse = await _personService.GetPersonById(addedPersonId);

        // Assert
        Assert.Equal(expectedPersonResponse, actualPersonResponse);
    }
    #endregion

    #region GetAllPersons
    [Fact]
    public async Task GetAllPersons_ListEmpty_ReturnEmptyCollection()
    {
        // Arrange
        List<PersonResponse> actualList = new();

        // Act
        actualList = await _personService.GetAllPersons();

        // Assert
        Assert.Empty(actualList);
    }

    [Fact]
    public async Task GetAllPersons_ListNotEmpty_ReturnAllCollection()
    {
        // Arrange
        CountryAddRequest singaporeAddRequest1 = new() { Name = "Singapore" };
        CountryAddRequest thailandAddRequest2 = new() { Name = "Thailand" };
        CountryResponse singaporeResponse = await _countryService.AddCountry(singaporeAddRequest1);
        CountryResponse thailandResponse = await _countryService.AddCountry(thailandAddRequest2);

        List<PersonAddRequest> personAddRequests = new()
        {
            new()
            {
                Name = "Jack",
                Address = "21st Jump Street",
                Email = "Jack@mailinator.com",
                Gender = GenderOption.Male,
                DateOfBirth = DateTime.Parse("1995-01-01"),
                ReceiveNewsLetters = false,
                CountryId = singaporeResponse.Id,
            },

            new()
            {
                Name = "Sylva",
                Address = "21st Jump Street",
                Email = "Sylva@mailinator.com",
                Gender = GenderOption.Male,
                DateOfBirth = DateTime.Parse("1995-01-01"),
                ReceiveNewsLetters = false,
                CountryId = thailandResponse.Id,
            },

            new()
            {
                Name = "Mary",
                Address = "21st Jump Street",
                Email = "Mary@mailinator.com",
                Gender = GenderOption.Female,
                DateOfBirth = DateTime.Parse("1995-01-01"),
                ReceiveNewsLetters = false,
                CountryId = singaporeResponse.Id,
            },
        };
        List<PersonResponse> expectedPersonList = new();
        foreach (var request in personAddRequests)
        {
            var personResponse = await _personService.AddPerson(request);
            expectedPersonList.Add(personResponse);
        }

        // Act
        List<PersonResponse> actualPersonList = await _personService.GetAllPersons();

        // Assert
        foreach (var expectedPerson in expectedPersonList)
        {
            Assert.Contains(expectedPerson, actualPersonList);
        }
    }
    #endregion

    #region GetFilteredPersons
    [Fact]
    public async Task GetFilteredPersons_ArgumentEmpty_ListEmpty_ReturnEmptyCollection()
    {
        // Arrange
        List<PersonResponse> actualPersonSearchList = new();

        string searchBy = nameof(Person.Name);
        string keyword = string.Empty;

        // Act
        actualPersonSearchList = await _personService.GetFilteredPersons(searchBy, keyword);

        // Assert
        Assert.Empty(actualPersonSearchList);
    }

    [Fact]
    public async Task GetFilteredPersons_ArgumentEmpty_ListNotEmpty_ReturnAllCollection()
    {
        // Arrange
        CountryAddRequest singaporeAddRequest1 = new() { Name = "Singapore" };
        CountryAddRequest thailandAddRequest2 = new() { Name = "Thailand" };
        CountryResponse singaporeResponse = await _countryService.AddCountry(singaporeAddRequest1);
        CountryResponse thailandResponse = await _countryService.AddCountry(thailandAddRequest2);

        List<PersonAddRequest> personAddRequests = new()
        {
            new()
            {
                Name = "Jack",
                Address = "21st Jump Street",
                Email = "Jack@mailinator.com",
                Gender = GenderOption.Male,
                DateOfBirth = DateTime.Parse("1995-01-01"),
                ReceiveNewsLetters = false,
                CountryId = singaporeResponse.Id,
            },

            new()
            {
                Name = "Sylva",
                Address = "21st Jump Street",
                Email = "Sylva@mailinator.com",
                Gender = GenderOption.Male,
                DateOfBirth = DateTime.Parse("1995-01-01"),
                ReceiveNewsLetters = false,
                CountryId = thailandResponse.Id,
            },

            new()
            {
                Name = "Mary",
                Address = "21st Jump Street",
                Email = "Mary@mailinator.com",
                Gender = GenderOption.Female,
                DateOfBirth = DateTime.Parse("1995-01-01"),
                ReceiveNewsLetters = false,
                CountryId = singaporeResponse.Id,
            },
        };
        List<PersonResponse> expectedPersonSearchList = new();
        foreach (var request in personAddRequests)
        {
            var personResponse = await _personService.AddPerson(request);
            expectedPersonSearchList.Add(personResponse);
        }

        string searchBy = nameof(Person.Name);
        string keyword = string.Empty;

        // Act
        List<PersonResponse> actualPersonSearchList = await _personService.GetFilteredPersons(searchBy, keyword);

        // Assert
        foreach (var expectedPerson in expectedPersonSearchList)
        {
            Assert.Contains(expectedPerson, actualPersonSearchList);
        }
    }

    [Fact]
    public async Task GetFilteredPersons_SearchByName_ListNotEmpty_ReturnMatchesCollection()
    {
        // Arrange
        CountryAddRequest singaporeAddRequest1 = new() { Name = "Singapore" };
        CountryAddRequest thailandAddRequest2 = new() { Name = "Thailand" };
        CountryResponse singaporeResponse = await _countryService.AddCountry(singaporeAddRequest1);
        CountryResponse thailandResponse = await _countryService.AddCountry(thailandAddRequest2);

        List<PersonAddRequest> personAddRequests = new()
        {
            new()
            {
                Name = "Mark",
                Address = "21st Jump Street",
                Email = "Jack@mailinator.com",
                Gender = GenderOption.Male,
                DateOfBirth = DateTime.Parse("1995-01-01"),
                ReceiveNewsLetters = false,
                CountryId = singaporeResponse.Id,
            },

            new()
            {
                Name = "Sylva",
                Address = "21st Jump Street",
                Email = "Sylva@mailinator.com",
                Gender = GenderOption.Male,
                DateOfBirth = DateTime.Parse("1995-01-01"),
                ReceiveNewsLetters = false,
                CountryId = thailandResponse.Id,
            },

            new()
            {
                Name = "Mary",
                Address = "21st Jump Street",
                Email = "Mary@mailinator.com",
                Gender = GenderOption.Female,
                DateOfBirth = DateTime.Parse("1995-01-01"),
                ReceiveNewsLetters = false,
                CountryId = singaporeResponse.Id,
            },
        };
        List<PersonResponse> expectedPersonSearchList = new();
        foreach (var request in personAddRequests)
        {
            var personResponse = await _personService.AddPerson(request);
            expectedPersonSearchList.Add(personResponse);
        }

        string searchBy = nameof(Person.Name);
        string keyword = "ma";

        // Act
        List<PersonResponse> actualPersonSearchList = await _personService.GetFilteredPersons(searchBy, keyword);

        // Assert
        foreach (var expectedPerson in expectedPersonSearchList)
            if (expectedPerson.Name!.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                Assert.Contains(expectedPerson, actualPersonSearchList);
    }
    #endregion

    #region GetSortedPersons
    [Fact]
    public async Task GetSortedPersons_ListEmpty_ReturnEmptyCollection()
    {
        // Arrange
        List<PersonResponse> allPersons = await _personService.GetAllPersons();
        string sortBy = nameof(Person.Name);
        SortOrderEnum sortOrder = SortOrderEnum.ASC;

        // Act
        var sortedPersonsList = await _personService.GetSortedPersons(allPersons, sortBy, sortOrder);

        // Assert
        Assert.Empty(sortedPersonsList);
    }

    [Fact]
    public async Task GetSortedPersons_SortByNameDescendingOrder_ListNotEmpty_ReturnSortedCollection()
    {
        // Arrange
        CountryAddRequest singaporeAddRequest1 = new() { Name = "Singapore" };
        CountryAddRequest thailandAddRequest2 = new() { Name = "Thailand" };
        CountryResponse singaporeResponse = await _countryService.AddCountry(singaporeAddRequest1);
        CountryResponse thailandResponse = await _countryService.AddCountry(thailandAddRequest2);

        List<PersonAddRequest> personAddRequests = new()
        {
            new()
            {
                Name = "Aaron",
                Address = "21st Jump Street",
                Email = "Jack@mailinator.com",
                Gender = GenderOption.Male,
                DateOfBirth = DateTime.Parse("1995-01-01"),
                ReceiveNewsLetters = false,
                CountryId = singaporeResponse.Id,
            },

            new()
            {
                Name = "Cindy",
                Address = "21st Jump Street",
                Email = "Sylva@mailinator.com",
                Gender = GenderOption.Male,
                DateOfBirth = DateTime.Parse("1995-01-01"),
                ReceiveNewsLetters = false,
                CountryId = thailandResponse.Id,
            },

            new()
            {
                Name = "Becka",
                Address = "21st Jump Street",
                Email = "Mary@mailinator.com",
                Gender = GenderOption.Female,
                DateOfBirth = DateTime.Parse("1995-01-01"),
                ReceiveNewsLetters = false,
                CountryId = singaporeResponse.Id,
            },
        };
        List<PersonResponse> addedPersonList = new();
        foreach (var request in personAddRequests)
        {
            var personResponse = await _personService.AddPerson(request);
            addedPersonList.Add(personResponse);
        }
        var expectedSortedPersonList = addedPersonList.OrderByDescending(p => p.Name).ToList();

        List<PersonResponse> allPersons = await _personService.GetAllPersons();
        string sortBy = nameof(Person.Name);
        SortOrderEnum sortOrder = SortOrderEnum.DESC;

        // Act
        List<PersonResponse> actualSortedPersonList = await _personService.GetSortedPersons(allPersons, sortBy, sortOrder);

        // Assert
        for (int i = 0; i < actualSortedPersonList.Count; i++)
            Assert.Equal(expectedSortedPersonList[i], actualSortedPersonList[i]);
    }
    #endregion

    #region UpdatePerson
    [Fact]
    public async Task UpdatePerson_RequestModelNull_ThrowsArgumentNullException()
    {
        // Arrange
        PersonUpdateRequest? requestModel = null;

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            // Act
            await _personService.UpdatePerson(requestModel);
        });
    }

    [Fact]
    public async Task UpdatePerson_IdNotExist_ThrowsArgumentException()
    {
        // Arrange
        PersonUpdateRequest? requestModel = new()
        {
            Id = Guid.Parse("22A72BBB-9B9A-4FF1-8EBC-D8986110E39C")
        };

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _personService.UpdatePerson(requestModel);
        });
    }

    [Fact]
    public async Task UpdatePerson_NameNull_ThrowsArgumentException()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new() { Name = "Singapore" };
        CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);

        PersonAddRequest personAddRequest = new()
        {
            Name = "Aaron",
            Address = "21st Jump Street",
            Email = "Jack@mailinator.com",
            Gender = GenderOption.Male,
            DateOfBirth = DateTime.Parse("1995-01-01"),
            ReceiveNewsLetters = false,
            CountryId = countryResponse.Id,
        };
        var personAddResponse = await _personService.AddPerson(personAddRequest);

        PersonUpdateRequest? requestModel = personAddResponse.ToPersonUpdateRequest();
        requestModel.Name = null;

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _personService.UpdatePerson(requestModel);
        });
    }

    [Fact]
    public async Task UpdatePerson_NameAndEmailUpdate_ReturnUpdatedPersonResponse()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new() { Name = "Singapore" };
        CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);

        PersonAddRequest personAddRequest = new()
        {
            Name = "Aaron",
            Address = "21st Jump Street",
            Email = "Aaron@mailinator.com",
            Gender = GenderOption.Male,
            DateOfBirth = DateTime.Parse("1995-01-01"),
            ReceiveNewsLetters = false,
            CountryId = countryResponse.Id,
        };
        var personAddResponse = await _personService.AddPerson(personAddRequest);

        PersonUpdateRequest? requestModel = personAddResponse.ToPersonUpdateRequest();
        requestModel.Name = "William";
        requestModel.Email = "william@mailinator.com";

        // Act
        PersonResponse actualPersonResponse = await _personService.UpdatePerson(requestModel);

        // Assert
        PersonResponse? getPersonResponse = await _personService.GetPersonById(actualPersonResponse.Id);
        Assert.Equal(getPersonResponse, actualPersonResponse);
    }
    #endregion

    #region DeletePerson
    [Fact]
    public async Task DeletePerson_IdExist_ReturnTrue()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new() { Name = "Singapore" };
        CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);

        PersonAddRequest personAddRequest = new()
        {
            Name = "Aaron",
            Address = "21st Jump Street",
            Email = "Aaron@mailinator.com",
            Gender = GenderOption.Male,
            DateOfBirth = DateTime.Parse("1995-01-01"),
            ReceiveNewsLetters = false,
            CountryId = countryResponse.Id,
        };
        var personAddResponse = await _personService.AddPerson(personAddRequest);
        
        Guid? id = personAddResponse.Id;

        // Act
        bool isDeleted = await _personService.DeletePerson(id);

        // Assert
        Assert.True(isDeleted);
    }

    [Fact]
    public async Task DeletePerson_IdNotExist_ReturnFalse()
    {
        // Arrange
        CountryAddRequest countryAddRequest = new() { Name = "Singapore" };
        CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);

        PersonAddRequest personAddRequest = new()
        {
            Name = "Aaron",
            Address = "21st Jump Street",
            Email = "Aaron@mailinator.com",
            Gender = GenderOption.Male,
            DateOfBirth = DateTime.Parse("1995-01-01"),
            ReceiveNewsLetters = false,
            CountryId = countryResponse.Id,
        };
        _personService.AddPerson(personAddRequest);

        Guid? id = Guid.Parse("C2E03BE8-1754-49EE-974C-DE6B2205651B");

        // Act
        bool isDeleted = await _personService.DeletePerson(id);

        // Assert
        Assert.False(isDeleted);
    }
    #endregion
}
