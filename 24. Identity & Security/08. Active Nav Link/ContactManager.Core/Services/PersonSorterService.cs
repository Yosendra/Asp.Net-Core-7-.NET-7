using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace Services;

public class PersonSorterService : IPersonSorterService
{
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<PersonSorterService> _logger;
    private readonly IDiagnosticContext _diagnosticContext;

    public PersonSorterService(
        IPersonRepository personRepository, 
        ILogger<PersonSorterService> logger, 
        IDiagnosticContext diagnosticContext)
    {
        _personRepository = personRepository;
        _logger = logger;
        _diagnosticContext = diagnosticContext;
    }

    public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderEnum sortOrder)
    {
        _logger.LogInformation("GetSortedPersons of PersonService");

        if (string.IsNullOrWhiteSpace(sortBy))
            return allPersons;

        List<PersonResponse> sortedPersonList = (sortBy, sortOrder) switch
        {
            (nameof(PersonResponse.Name), SortOrderEnum.ASC)
                => allPersons.OrderBy(p => p.Name).ToList(),
            (nameof(PersonResponse.Name), SortOrderEnum.DESC)
                => allPersons.OrderByDescending(p => p.Name).ToList(),

            (nameof(PersonResponse.Email), SortOrderEnum.ASC)
                => allPersons.OrderBy(p => p.Email).ToList(),
            (nameof(PersonResponse.Email), SortOrderEnum.DESC)
                => allPersons.OrderByDescending(p => p.Email).ToList(),

            (nameof(PersonResponse.DateOfBirth), SortOrderEnum.ASC)
                => allPersons.OrderBy(p => p.DateOfBirth).ToList(),
            (nameof(PersonResponse.DateOfBirth), SortOrderEnum.DESC)
                => allPersons.OrderByDescending(p => p.DateOfBirth).ToList(),

            (nameof(PersonResponse.Age), SortOrderEnum.ASC)
                => allPersons.OrderBy(p => p.Age).ToList(),
            (nameof(PersonResponse.Age), SortOrderEnum.DESC)
                => allPersons.OrderByDescending(p => p.Age).ToList(),

            (nameof(PersonResponse.Gender), SortOrderEnum.ASC)
                => allPersons.OrderBy(p => p.Gender).ToList(),
            (nameof(PersonResponse.Gender), SortOrderEnum.DESC)
                => allPersons.OrderByDescending(p => p.Gender).ToList(),

            (nameof(PersonResponse.CountryName), SortOrderEnum.ASC)
                => allPersons.OrderBy(p => p.CountryName).ToList(),
            (nameof(PersonResponse.CountryName), SortOrderEnum.DESC)
                => allPersons.OrderByDescending(p => p.CountryName).ToList(),

            (nameof(PersonResponse.Address), SortOrderEnum.ASC)
                => allPersons.OrderBy(p => p.Address).ToList(),
            (nameof(PersonResponse.Address), SortOrderEnum.DESC)            
                => allPersons.OrderByDescending(p => p.Address).ToList(),

            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderEnum.ASC)
                => allPersons.OrderBy(p => p.ReceiveNewsLetters).ToList(),
            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderEnum.DESC)
                => allPersons.OrderByDescending(p => p.ReceiveNewsLetters).ToList(),

            _ => allPersons
        };

        return sortedPersonList;
    }
}
