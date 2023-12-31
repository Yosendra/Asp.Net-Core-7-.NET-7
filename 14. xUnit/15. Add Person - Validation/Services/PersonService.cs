using System.ComponentModel.DataAnnotations;
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

        //if (string.IsNullOrWhiteSpace(requestModel.Name))
        //{
        //    string errorMessage = string.Format("{0} cannot be blank", nameof(requestModel.Name));
        //    throw new ArgumentException(errorMessage);
        //}

        // Model Validation - using the attribute we have defined in the PersonAddRequest properties

        //ValidationContext validationContext = new(requestModel);
        //List<ValidationResult> validationResults = new();
        //bool isRequestModelValid = Validator.TryValidateObject(requestModel, validationContext, validationResults, true);
        //if (!isRequestModelValid)
        //{
        //    string? errorMessage = validationResults.FirstOrDefault()?.ErrorMessage;
        //    throw new ArgumentException(errorMessage);
        //}

        ValidationHelper.ModelValidation(requestModel);

        Person person = requestModel.ToPerson();
        person.Id = Guid.NewGuid();
        _personDataStore.Add(person);

        return ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse> GetAllPerson()
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
