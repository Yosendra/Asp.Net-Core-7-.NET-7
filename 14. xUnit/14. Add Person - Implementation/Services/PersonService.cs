using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

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
        // Check if requestModel is not null
        ArgumentNullException.ThrowIfNull(requestModel);

        // Validate all properties of requestModel
        if (string.IsNullOrWhiteSpace(requestModel.Name))
        {
            string errorMessage = string.Format("{0} cannot be blank", nameof(requestModel.Name));
            throw new ArgumentException(errorMessage);
        }

        // Convert requestModel to Person entity object
        Person person = requestModel.ToPerson();

        // Generate new id for entity object
        person.Id = Guid.NewGuid();

        // Save the entity to the data store
        _personDataStore.Add(person);

        // Return entity as PersonResponse object along with its Id
        // case: PersonResponse contains CountryName, since we only have CountryId in person, how do we get CountryName here?
        // Use CountryService

        //var personResponse = person.ToPersonResponse();

        //Guid countryId = personResponse.CountryId;
        //CountryResponse? countryResponse = _countryService.GetCountryById(countryId);
        //personResponse.CountryName = countryResponse?.Name;

        //personResponse.CountryName = _countryService.GetCountryById(personResponse.CountryId)?.Name;

        return ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse> GetAllPerson()
    {
        throw new NotImplementedException();
    }

    #region Private Methods
    // Refactor to follow Single Responsibility Principle and also Reusability sake
    private PersonResponse ConvertPersonToPersonResponse(Person person)
    {
        var personResponse = person.ToPersonResponse();
        personResponse.CountryName = _countryService.GetCountryById(personResponse.CountryId)?.Name;
        return personResponse;
    }
    #endregion
}
