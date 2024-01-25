using System.Linq.Expressions;
using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

/* Mocking
 *   Mock<IPersonRepository>       Used to mock the methods of IPersonRepository
 *   
 *   IPersonRepository             Represents the mocked object that was crated by Mock<T>
 *   
 *   in GetPersonById_Exist_ReturnCorrectPersonResponse() we delete invoking AddCountry() because we will mock.
 *   Principle in unit test, you should not test more than one method at a time.
 **/


namespace CRUDTests;

public class PersonServiceTest
{
    #region Fields
    private readonly IPersonService _personService;
    private readonly ICountryService _countryService;

    private readonly IPersonRepository _personRepository;   // notice this
    private readonly Mock<IPersonRepository> _personRepositoryMock; // notice this

    private readonly IFixture _fixture;
    #endregion

    #region Constructor
    public PersonServiceTest()
    {
        List<Country> countryInitialData = new();
        List<Person> personInitialData = new();

        // Mock the DbContext
        var dbContextOption = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        DbContextMock<ApplicationDbContext> dbContextMock = new(dbContextOption);

        // Mock the DbSet functionality and load the initial data for countries and person
        dbContextMock.CreateDbSetMock(t => t.Countries, countryInitialData);
        dbContextMock.CreateDbSetMock(t => t.Persons, personInitialData);

        ApplicationDbContext dbContext = dbContextMock.Object;

        _personRepositoryMock = new Mock<IPersonRepository>();      // notice this
        _personRepository = _personRepositoryMock.Object;           // notice this

        _countryService = new CountryService(null);
        _personService = new PersonService(_personRepository);
        _fixture = new Fixture();
    }
    #endregion

    #region AddPerson
    [Fact]
    public async Task AddPerson_RequestModelNull_ThrowsArgumentNullException()
    {
        // Arrange
        PersonAddRequest? requestModel = null;

        // Act
        Func<Task> action = async () => await _personService.AddPerson(requestModel);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddPerson_NameNull_ThrowsArgumentException()
    {
        // Arrange
        var requestModel = _fixture.Build<PersonAddRequest>()
            .With(p => p.Name, null as string)
            .Create();

        // Act
        Func<Task> action = async () => await _personService.AddPerson(requestModel);
        
        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddPerson_Success_ReturnPersonResponseWithId()
    {
        // Arrange
        var requestModel = _fixture.Build<PersonAddRequest>()
            .With(p => p.Email, "someone@example.com")
            .Create();

        Person person = requestModel.ToPerson();    // notice this
        PersonResponse expected = person.ToPersonResponse();    // notice this

        // if we supply any argument value to the AddPerson method,
        // it should return the same return value
        _personRepositoryMock
            .Setup(repo => repo.AddPerson(It.IsAny<Person>()))
            .ReturnsAsync(person);

        // Act
        PersonResponse actual = await _personService.AddPerson(requestModel);

        // Assert
        expected.Id = actual.Id;
        actual.Id.Should().NotBeNull();
        actual.Should().Be(expected);
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
        response.Should().BeNull();
    }

    [Fact]
    public async Task GetPersonById_Exist_ReturnCorrectPersonResponse()
    {
        // Arrange
        var person = _fixture.Build<Person>()
            .With(p => p.Email, "email@example.com")
            .With(p => p.Country, null as Country)  // we need to add this because it contain circular dependency (the navigation property)
            .Create();
        PersonResponse expected = person.ToPersonResponse();

        _personRepositoryMock
            .Setup(repo => repo.GetPersonById(It.IsAny<Guid>()))
            .ReturnsAsync(person);

        // Act
        PersonResponse? actual = await _personService.GetPersonById(person.Id);

        // Assert
        actual.Should().Be(expected);
    }
    #endregion

    #region GetAllPersons
    [Fact]
    public async Task GetAllPersons_ListEmpty_ReturnEmptyCollection()
    {
        // Arrange
        List<Person> personList = new();

        _personRepositoryMock
            .Setup(repo => repo.GetAllPersons())
            .ReturnsAsync(personList);

        // Act
        List<PersonResponse> actualList = await _personService.GetAllPersons();

        // Assert
        actualList.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllPersons_ListNotEmpty_ReturnAllCollection()
    {
        // Arrange
        List<Person> personList = new()
        {
            _fixture.Build<Person>()
                .With(p => p.Email, "someone1@mailinator.com")
                .With(p => p.Country, null as Country)  // we need to add this because it contain circular dependency (the navigation property)
                .Create(),

            _fixture.Build<Person>()
                .With(p => p.Email, "someone2@mailinator.com")
                .With(p => p.Country, null as Country)  // we need to add this because it contain circular dependency (the navigation property)
                .Create(),

            _fixture.Build<Person>()
                .With(p => p.Email, "someone3@mailinator.com")
                .With(p => p.Country, null as Country)  // we need to add this because it contain circular dependency (the navigation property)
                .Create(),
        };
        List<PersonResponse> expected = personList.Select(p => p.ToPersonResponse()).ToList();

        _personRepositoryMock
            .Setup(repo => repo.GetAllPersons())
            .ReturnsAsync(personList);

        // Act
        List<PersonResponse> actual = await _personService.GetAllPersons();

        // Assert
        actual.Should().BeEquivalentTo(expected);
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
        actualPersonSearchList.Should().BeEmpty();
    }

    [Fact]
    public async Task GetFilteredPersons_ArgumentEmpty_ListNotEmpty_ReturnAllCollection()
    {
        // Arrange
        List<Person> personList = new()
        {
            _fixture.Build<Person>()
                .With(p => p.Email, "someone1@mailinator.com")
                .With(p => p.Country, null as Country)  // we need to add this because it contain circular dependency (the navigation property)
                .Create(),

            _fixture.Build<Person>()
                .With(p => p.Email, "someone2@mailinator.com")
                .With(p => p.Country, null as Country)  // we need to add this because it contain circular dependency (the navigation property)
                .Create(),

            _fixture.Build<Person>()
                .With(p => p.Email, "someone3@mailinator.com")
                .With(p => p.Country, null as Country)  // we need to add this because it contain circular dependency (the navigation property)
                .Create(),
        };
        List<PersonResponse> expected = personList.Select(p => p.ToPersonResponse()).ToList();

        _personRepositoryMock
            .Setup(repo => repo.GetFilteredPerson(It.IsAny<Expression<Func<Person, bool>>>()))
            .ReturnsAsync(personList);

        string searchBy = nameof(Person.Name);
        string keyword = string.Empty;

        // Act
        List<PersonResponse> actualPersonSearchList = await _personService.GetFilteredPersons(searchBy, keyword);

        // Assert
        actualPersonSearchList.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetFilteredPersons_SearchByName_ListNotEmpty_ReturnMatchesCollection()
    {
        // Arrange
        List<Person> personList = new()
        {
            _fixture.Build<Person>()
                .With(p => p.Email, "someone1@mailinator.com")
                .With(p => p.Country, null as Country)  // we need to add this because it contain circular dependency (the navigation property)
                .Create(),

            _fixture.Build<Person>()
                .With(p => p.Email, "someone2@mailinator.com")
                .With(p => p.Country, null as Country)  // we need to add this because it contain circular dependency (the navigation property)
                .Create(),

            _fixture.Build<Person>()
                .With(p => p.Email, "someone3@mailinator.com")
                .With(p => p.Country, null as Country)  // we need to add this because it contain circular dependency (the navigation property)
                .Create(),
        };
        List<PersonResponse> expected = personList.Select(p => p.ToPersonResponse()).ToList();

        _personRepositoryMock
            .Setup(repo => repo.GetFilteredPerson(It.IsAny<Expression<Func<Person, bool>>>()))
            .ReturnsAsync(personList);

        string searchBy = nameof(Person.Name);
        string keyword = "one";

        // Act
        List<PersonResponse> actualPersonSearchList = await _personService.GetFilteredPersons(searchBy, keyword);

        // Assert
        actualPersonSearchList.Should().BeEquivalentTo(expected);
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
        List<PersonResponse> sortedPersonsList = 
            await _personService.GetSortedPersons(allPersons, sortBy, sortOrder);

        // Assert
        sortedPersonsList.Should().BeEmpty();
    }

    [Fact]
    public async Task GetSortedPersons_SortByNameDescendingOrder_ListNotEmpty_ReturnSortedCollection()
    {
        // Arrange
        var countryAddRequest1 = _fixture.Create<CountryAddRequest>();
        var countryAddRequest2 = _fixture.Create<CountryAddRequest>();
        CountryResponse countryResponse1 = await _countryService.AddCountry(countryAddRequest1);
        CountryResponse countryResponse2 = await _countryService.AddCountry(countryAddRequest2);

        List<PersonAddRequest> personAddRequests = new()
        {
            _fixture.Build<PersonAddRequest>()
                .With(p => p.Name, "Someone1")
                .With(p => p.Email, "someone1@mailinator.com")
                .With(p => p.CountryId, countryResponse1.Id)
                .Create(),

            _fixture.Build<PersonAddRequest>()
                .With(p => p.Name, "Someone2")
                .With(p => p.Email, "someone2@mailinator.com")
                .With(p => p.CountryId, countryResponse2.Id)
                .Create(),

            _fixture.Build<PersonAddRequest>()
                .With(p => p.Name, "Someone3")
                .With(p => p.Email, "someone3@mailinator.com")
                .With(p => p.CountryId, countryResponse1.Id)
                .Create(),
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
        List<PersonResponse> actualSortedPersonList = 
            await _personService.GetSortedPersons(allPersons, sortBy, sortOrder);

        // Assert
        actualSortedPersonList.Should().BeInDescendingOrder(p => p.Name);
    }
    #endregion

    #region UpdatePerson
    [Fact]
    public async Task UpdatePerson_RequestModelNull_ThrowsArgumentNullException()
    {
        // Arrange
        PersonUpdateRequest? requestModel = null;

        // Act
        Func<Task> action = async () => await _personService.UpdatePerson(requestModel);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdatePerson_IdNotExist_ThrowsArgumentException()
    {
        // Arrange
        var requestModel = _fixture.Create<PersonUpdateRequest>();

        // Act
        Func<Task> action = async () => await _personService.UpdatePerson(requestModel);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdatePerson_NameNull_ThrowsArgumentException()
    {
        // Arrange
        var countryAddRequest = _fixture.Create<CountryAddRequest>();
        CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);

        var personAddRequest = 
            _fixture.Build<PersonAddRequest>()
                .With(p => p.Name, "Someone")
                .With(p => p.Email, "someone@mailinator.com")
                .With(p => p.CountryId, countryResponse.Id)
                .Create();
        PersonResponse personAddResponse = await _personService.AddPerson(personAddRequest);

        PersonUpdateRequest? requestModel = personAddResponse.ToPersonUpdateRequest();
        requestModel.Name = null;

        // Act
        Func<Task> action = async () => await _personService.UpdatePerson(requestModel);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdatePerson_NameAndEmailUpdate_ReturnUpdatedPersonResponse()
    {
        // Arrange
        var countryAddRequest = _fixture.Create<CountryAddRequest>();
        CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);

        var personAddRequest = 
            _fixture.Build<PersonAddRequest>()
                .With(p => p.Name, "Someone")
                .With(p => p.Email, "someone@mailinator.com")
                .With(p => p.CountryId, countryResponse.Id)
                .Create();
        PersonResponse personAddResponse = await _personService.AddPerson(personAddRequest);

        PersonUpdateRequest? requestModel = personAddResponse.ToPersonUpdateRequest();
        requestModel.Name = "William";
        requestModel.Email = "william@mailinator.com";

        // Act
        PersonResponse actualPersonResponse = 
            await _personService.UpdatePerson(requestModel);

        // Assert
        PersonResponse? getPersonResponse = await _personService.GetPersonById(actualPersonResponse.Id);
        actualPersonResponse.Should().Be(getPersonResponse);
    }
    #endregion

    #region DeletePerson
    [Fact]
    public async Task DeletePerson_IdExist_ReturnTrue()
    {
        // Arrange
        var countryAddRequest = _fixture.Create<CountryAddRequest>();
        CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);

        var personAddRequest = 
            _fixture.Build<PersonAddRequest>()
                .With(p => p.Name, "Someone")
                .With(p => p.Email, "someone@mailinator.com")
                .With(p => p.CountryId, countryResponse.Id)
                .Create();
        PersonResponse personAddResponse = await _personService.AddPerson(personAddRequest);

        Guid? id = personAddResponse.Id;

        // Act
        bool isDeleted = await _personService.DeletePerson(id);

        // Assert
        isDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task DeletePerson_IdNotExist_ReturnFalse()
    {
        // Arrange
        var countryAddRequest = _fixture.Create<CountryAddRequest>();
        CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);

        var personAddRequest = 
            _fixture.Build<PersonAddRequest>()
                .With(p => p.Name, "Someone")
                .With(p => p.Email, "someone@mailinator.com")
                .With(p => p.CountryId, countryResponse.Id)
                .Create();
        await _personService.AddPerson(personAddRequest);

        Guid? id = Guid.Parse("C2E03BE8-1754-49EE-974C-DE6B2205651B");

        // Act
        bool isDeleted = await _personService.DeletePerson(id);

        // Assert
        isDeleted.Should().BeFalse();
    }
    #endregion
}
