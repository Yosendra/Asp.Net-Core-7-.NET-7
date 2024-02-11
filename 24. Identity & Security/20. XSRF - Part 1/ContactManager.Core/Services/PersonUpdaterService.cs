using Entities;
using Exceptions;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helper;

namespace Services;

public class PersonUpdaterService : IPersonUpdaterService
{
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<PersonUpdaterService> _logger;
    private readonly IDiagnosticContext _diagnosticContext;

    public PersonUpdaterService(
        IPersonRepository personRepository, 
        ILogger<PersonUpdaterService> logger, 
        IDiagnosticContext diagnosticContext)
    {
        _personRepository = personRepository;
        _logger = logger;
        _diagnosticContext = diagnosticContext;
    }

    public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);

        ValidationHelper.ModelValidation(requestModel);

        Person? matchingPerson = await _personRepository.GetPersonById(requestModel.Id.Value);
        if (matchingPerson == null)
            throw new InvalidPersonIdException("Given id doesn't exist");

        matchingPerson.Name = requestModel.Name;
        matchingPerson.Email = requestModel.Email;
        matchingPerson.DateOfBirth = requestModel.DateOfBirth;
        matchingPerson.Gender = requestModel.Gender.ToString();
        matchingPerson.CountryId = requestModel.CountryId;
        matchingPerson.Address = requestModel.Address;
        matchingPerson.ReceiveNewsLetters = requestModel.ReceiveNewsLetters;

        await _personRepository.UpdatePerson(matchingPerson);

        return matchingPerson.ToPersonResponse();
    }
}
