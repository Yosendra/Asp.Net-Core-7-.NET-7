using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts;

namespace Services;

public class PersonDeleterService : IPersonDeleterService
{
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<PersonDeleterService> _logger;
    private readonly IDiagnosticContext _diagnosticContext;

    public PersonDeleterService(
        IPersonRepository personRepository, 
        ILogger<PersonDeleterService> logger, 
        IDiagnosticContext diagnosticContext)
    {
        _personRepository = personRepository;
        _logger = logger;
        _diagnosticContext = diagnosticContext;
    }

    public async Task<bool> DeletePerson(Guid? id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Person? person = await _personRepository.GetPersonById(id.Value);
        if (person == null)
            return false;

        await _personRepository.DeletePersonById(person.Id!.Value);

        return true;
    }
}
