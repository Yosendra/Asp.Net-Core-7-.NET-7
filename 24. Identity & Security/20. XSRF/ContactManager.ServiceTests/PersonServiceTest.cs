using System.Linq.Expressions;
using AutoFixture;
using Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit;

namespace CRUDTests;

public class PersonServiceTest
{
    #region Fields
    private readonly IPersonService _personService;

    private readonly IPersonRepository _personRepository;
    private readonly Mock<IPersonRepository> _personRepositoryMock;

    private readonly IFixture _fixture;
    #endregion

    #region Constructor
    public PersonServiceTest()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();
        _personRepository = _personRepositoryMock.Object;

        var diagnosticContext = new Mock<IDiagnosticContext>();
        var logger = new Mock<ILogger<PersonService>>();

        _personService = new PersonService(_personRepository, logger.Object, diagnosticContext.Object);
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

        Person person = requestModel.ToPerson();
        PersonResponse expected = person.ToPersonResponse();

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
            .With(p => p.Country, null as Country)
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
                .With(p => p.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(p => p.Email, "someone2@mailinator.com")
                .With(p => p.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(p => p.Email, "someone3@mailinator.com")
                .With(p => p.Country, null as Country)
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
        List<Person> personList = new();

        _personRepositoryMock
            .Setup(repo => repo.GetFilteredPerson(It.IsAny<Expression<Func<Person, bool>>>()))
            .ReturnsAsync(personList);

        string searchBy = nameof(Person.Name);
        string keyword = string.Empty;

        // Act
        List<PersonResponse> actual = await _personService.GetFilteredPersons(searchBy, keyword);

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task GetFilteredPersons_ArgumentEmpty_ListNotEmpty_ReturnAllCollection()
    {
        // Arrange
        List<Person> personList = new()
        {
            _fixture.Build<Person>()
                .With(p => p.Email, "someone1@mailinator.com")
                .With(p => p.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(p => p.Email, "someone2@mailinator.com")
                .With(p => p.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(p => p.Email, "someone3@mailinator.com")
                .With(p => p.Country, null as Country)
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
                .With(p => p.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(p => p.Email, "someone2@mailinator.com")
                .With(p => p.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(p => p.Email, "someone3@mailinator.com")
                .With(p => p.Country, null as Country)
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
        List<Person> personList = new();

        _personRepositoryMock
            .Setup(repo => repo.GetAllPersons())
            .ReturnsAsync(personList);

        List<PersonResponse> allPersons = await _personService.GetAllPersons();
        string sortBy = nameof(Person.Name);
        SortOrderEnum sortOrder = SortOrderEnum.ASC;

        // Act
        List<PersonResponse> actual = await _personService.GetSortedPersons(allPersons, sortBy, sortOrder);

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public async Task GetSortedPersons_SortByNameDescendingOrder_ListNotEmpty_ReturnSortedCollection()
    {
        // Arrange
        List<Person> personList = new()
        {
            _fixture.Build<Person>()
                .With(p => p.Email, "someone1@mailinator.com")
                .With(p => p.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(p => p.Email, "someone2@mailinator.com")
                .With(p => p.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(p => p.Email, "someone3@mailinator.com")
                .With(p => p.Country, null as Country)
                .Create(),
        };
        List<PersonResponse> expected = personList.Select(p => p.ToPersonResponse()).ToList();

        _personRepositoryMock
            .Setup(repo => repo.GetAllPersons())
            .ReturnsAsync(personList);

        List<PersonResponse> allPersons = await _personService.GetAllPersons();
        string sortBy = nameof(Person.Name);
        SortOrderEnum sortOrder = SortOrderEnum.DESC;

        // Act
        List<PersonResponse> actualSortedPersonList = await _personService.GetSortedPersons(allPersons, sortBy, sortOrder);

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
        var requestModel = _fixture.Build<PersonUpdateRequest>()
            .With(p => p.Email, "someone@example.com")
            .Create();

        _personRepositoryMock
            .Setup(repo => repo.GetPersonById(It.IsAny<Guid>()))
            .ReturnsAsync(null as Person);

        // Act
        Func<Task> action = async () => await _personService.UpdatePerson(requestModel);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdatePerson_NameNull_ThrowsArgumentException()
    {
        // Arrange
        Person person = _fixture.Build<Person>()
                .With(p => p.Email, "someone@mailinator.com")
                .With(p => p.Gender, GenderOption.Male.ToString())
                .With(p => p.Country, null as Country)
                .Create();

        PersonUpdateRequest? requestModel = person.ToPersonResponse().ToPersonUpdateRequest();
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
        Person person = _fixture.Build<Person>()
                .With(p => p.Email, "someone@mailinator.com")
                .With(p => p.Gender, GenderOption.Male.ToString())
                .With(p => p.Country, null as Country)
                .Create();
        
        _personRepositoryMock
            .Setup(repo => repo.GetPersonById(It.IsAny<Guid>()))
            .ReturnsAsync(person);

        _personRepositoryMock
            .Setup(repo => repo.UpdatePerson(It.IsAny<Person>()))
            .ReturnsAsync(person);

        PersonResponse expected = person.ToPersonResponse();
        PersonUpdateRequest? requestModel = expected.ToPersonUpdateRequest();

        // Act
        PersonResponse actual = await _personService.UpdatePerson(requestModel);

        // Assert
        actual.Should().Be(expected);
    }
    #endregion

    #region DeletePerson
    [Fact]
    public async Task DeletePerson_IdExist_ReturnTrue()
    {
        // Arrange
        Person person = _fixture.Build<Person>()
            .With(p => p.Email, "someone@mailinator.com")
            .With(p => p.Country, null as Country)
            .Create();

        _personRepositoryMock
            .Setup(repo => repo.GetPersonById(It.IsAny<Guid>()))
            .ReturnsAsync(person);

        _personRepositoryMock
            .Setup(repo => repo.DeletePersonById(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        Guid? id = person.Id;

        // Act
        bool isDeleted = await _personService.DeletePerson(id);

        // Assert
        isDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task DeletePerson_IdNotExist_ReturnFalse()
    {
        // Arrange
        _personRepositoryMock
           .Setup(repo => repo.GetPersonById(It.IsAny<Guid>()))
           .ReturnsAsync(null as Person);

        Guid? id = Guid.Parse("C2E03BE8-1754-49EE-974C-DE6B2205651B");

        // Act
        bool isDeleted = await _personService.DeletePerson(id);

        // Assert
        isDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task DeletePerson_IdNull_ThrowsArgumentNullException()
    {
        // Arrange
        Guid? id = null;

        // Act
        Func<Task> action = async () => await _personService.DeletePerson(id);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }
    #endregion
}
