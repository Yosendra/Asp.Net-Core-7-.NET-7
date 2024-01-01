﻿using Entities;
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

    public List<PersonResponse> GetAllPerson()
    {
        throw new NotImplementedException();
    }

    public PersonResponse? GetPersonById(Guid? id)
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