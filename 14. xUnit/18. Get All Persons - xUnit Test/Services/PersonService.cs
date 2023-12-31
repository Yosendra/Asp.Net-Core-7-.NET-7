using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
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
        // Check if id is not null
        if (id == null)
            return null;

        // Get the matching person based on id from data store
        Person? person = _personDataStore.FirstOrDefault(p => p.Id == id);

        // Convert matching person object from Person to PersonResponse type
        // Return PersonResponse object
        return person != null
            ? ConvertPersonToPersonResponse(person)
            : null;
    }

    public List<PersonResponse> GetAllPersons()
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
