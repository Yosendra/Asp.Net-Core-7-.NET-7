using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helper;

namespace Services;

public class PersonAdderService : IPersonAdderService
{
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<PersonAdderService> _logger;
    private readonly IDiagnosticContext _diagnosticContext;

    public PersonAdderService(
        IPersonRepository personRepository, 
        ILogger<PersonAdderService> logger, 
        IDiagnosticContext diagnosticContext)
    {
        _personRepository = personRepository;
        _logger = logger;
        _diagnosticContext = diagnosticContext;
    }

    public async Task<PersonResponse> AddPerson(PersonAddRequest? requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);

        ValidationHelper.ModelValidation(requestModel);

        Person person = requestModel.ToPerson();
        person.Id = Guid.NewGuid();

        await _personRepository.AddPerson(person);

        return person.ToPersonResponse();
    }
}
