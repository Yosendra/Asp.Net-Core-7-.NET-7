using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helper;

namespace Services;

public class PersonService : IPersonService
{
    // Data Store
    private readonly List<Person> _personDataStore;

    // Service
    private readonly ICountryService _countryService;

    public PersonService()
    {
        _personDataStore = new();
        _countryService = new CountryService();
    }

    public PersonResponse AddPerson(PersonAddRequest? requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);

        ValidationHelper.ModelValidation(requestModel);

        Person person = requestModel.ToPerson();
        person.Id = Guid.NewGuid();
        _personDataStore.Add(person);

        return ConvertPersonToPersonResponse(person);
    }

    public PersonResponse? GetPersonById(Guid? id)
    {
        if (id == null)
            return null;

        Person? person = _personDataStore.FirstOrDefault(p => p.Id == id);

        return person != null
            ? ConvertPersonToPersonResponse(person)
            : null;
    }

    public List<PersonResponse> GetAllPersons()
    {
        return _personDataStore.Select(p => p.ToPersonResponse()).ToList();
    }

    public List<PersonResponse> GetFilteredPersons(string searchBy, string? keyword)
    {
        List<PersonResponse> allPersons = GetAllPersons();
        List<PersonResponse> matchingPersons = allPersons;

        // Check if searchBy is not null
        if (string.IsNullOrWhiteSpace(searchBy) || (!string.IsNullOrWhiteSpace(searchBy) && string.IsNullOrWhiteSpace(keyword))) 
            return matchingPersons;

        // Get matching persons from data store based on searchBy and keyword
        // Convert the matching persons from Person type to PersonResponse type (this one is already done by GetAllPersons() above)
        // Return all matching PersonResponse object
        switch (searchBy)
        {
            // Assume all fields is not nullable excepts Address

            case nameof(Person.Name):
                matchingPersons = allPersons.Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case nameof(Person.Email):
                matchingPersons = allPersons.Where(p => p.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case nameof(Person.DateOfBirth):
                matchingPersons = allPersons.Where(p => p.DateOfBirth.Value
                                                                     .ToString("dd MMMM yyyy")
                                                                     .Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                                                     .ToList();
                break;

            case nameof(Person.Gender):
                matchingPersons = allPersons.Where(p => p.Gender.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case nameof(Person.CountryId):
                matchingPersons = allPersons.Where(p => p.CountryName.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case nameof(Person.Address):    // we assume this one is nullable
                matchingPersons = allPersons.Where(p => string.IsNullOrWhiteSpace(p.Address) 
                                                        ? false
                                                        : p.Address.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            default:
                matchingPersons = allPersons;
                break;
        }

        return matchingPersons;
    }

    public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderEnum sortOrder)
    {
        throw new NotImplementedException();
    }

    #region Private Methods
    private PersonResponse ConvertPersonToPersonResponse(Person person)
    {
        var personResponse = person.ToPersonResponse();
        personResponse.CountryName = _countryService.GetCountryById(personResponse.CountryId)?.Name;
        return personResponse;
    }
    #endregion
}
