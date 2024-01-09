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

    public PersonService(bool initialize = true)    // Pass false in unit test
    {
        _personDataStore = new();
        _countryService = new CountryService();

        if (initialize)
        {
            _personDataStore.AddRange(new List<Person>
            {
                new()
                {
                    Id = Guid.Parse("BD84B33B-507B-42C0-BFE8-A815D9316BCB"),
                    Name = "Ruthann",
                    Email = "rheaney0@ed.gov",
                    DateOfBirth = DateTime.Parse("1997-02-22"),
                    Gender = GenderOption.Female.ToString(),
                    Address = "7737 Heath Alley",
                    ReceiveNewsLetters = false,
                    CountryId = Guid.Parse("D97ADA24-50A2-4F5D-B88D-7588F40F0351"),
                },

                new()
                {
                    Id = Guid.Parse("734EBE90-0219-43A5-B552-76999CD1FCC2"),
                    Name = "Michal",
                    Email = "mcurman1@networkadvertising.org",
                    DateOfBirth = DateTime.Parse("1996-05-20"),
                    Gender = GenderOption.Female.ToString(),
                    Address = "9510 Johnson Center",
                    ReceiveNewsLetters = true,
                    CountryId = Guid.Parse("C7FA54B0-B8BD-4B78-B151-738961031B06"),
                },

                new()
                {
                    Id = Guid.Parse("5F171B8C-80AC-4B31-8C7B-11A906883CA1"),
                    Name = "Stacy",
                    Email = "smcmurraya2@si.edu",
                    DateOfBirth = DateTime.Parse("1994-10-18"),
                    Gender = GenderOption.Male.ToString(),
                    Address = "2 Muir Plaza",
                    ReceiveNewsLetters = true,
                    CountryId = Guid.Parse("BF61660F-6320-4C95-A752-DF655D29A2DE"),
                },

                new()
                {
                    Id = Guid.Parse("36219188-125B-4D56-B2C9-24379C996869"),
                    Name = "Leon",
                    Email = "ldumbrall3@squidoo.com",
                    DateOfBirth = DateTime.Parse("1993-12-10"),
                    Gender = GenderOption.Male.ToString(),
                    Address = "971 Onsgard Court",
                    ReceiveNewsLetters = true,
                    CountryId = Guid.Parse("AD54C2C6-088E-4393-B87C-F30C04CFF344"),
                },

                new()
                {
                    Id = Guid.Parse("FB43B429-A549-4043-AE30-527D1AEC1F02"),
                    Name = "Minta",
                    Email = "mgeistbeck4@google.cn",
                    DateOfBirth = DateTime.Parse("1992-02-29"),
                    Gender = GenderOption.Female.ToString(),
                    Address = "3 Clyde Gallagher Court",
                    ReceiveNewsLetters = false,
                    CountryId = Guid.Parse("F6B32853-5BFC-4655-A7EA-A86DBACE8DDB"),
                },

                new()
                {
                    Id = Guid.Parse("502CE957-FEB6-4C51-931E-5C3ABFE1CE20"),
                    Name = "Tulley",
                    Email = "tokennavain5@umich.edu",
                    DateOfBirth = DateTime.Parse("1994-05-17"),
                    Gender = GenderOption.Male.ToString(),
                    Address = "0678 New Castle Drive",
                    ReceiveNewsLetters = false,
                    CountryId = Guid.Parse("D97ADA24-50A2-4F5D-B88D-7588F40F0351"),
                }
            });
        }
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
        return _personDataStore.Select(p => ConvertPersonToPersonResponse(p)).ToList(); // use the private method instead of .ToPersonResponse()
    }

    public List<PersonResponse> GetFilteredPersons(string searchBy, string? keyword)
    {
        List<PersonResponse> allPersons = GetAllPersons();
        List<PersonResponse> matchingPersons = allPersons;

        if (string.IsNullOrWhiteSpace(searchBy) || (!string.IsNullOrWhiteSpace(searchBy) && string.IsNullOrWhiteSpace(keyword))) 
            return matchingPersons;

        switch (searchBy)
        {
            case nameof(PersonResponse.Name):
                matchingPersons = allPersons.Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case nameof(PersonResponse.Email):
                matchingPersons = allPersons.Where(p => p.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case nameof(PersonResponse.DateOfBirth):
                matchingPersons = allPersons.Where(p => p.DateOfBirth.Value
                                                                     .ToString("dd MMMM yyyy")
                                                                     .Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                                                     .ToList();
                break;

            case nameof(PersonResponse.Gender):
                matchingPersons = allPersons.Where(p => p.Gender.Equals(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case nameof(PersonResponse.CountryId):
                matchingPersons = allPersons.Where(p => p.CountryName.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case nameof(PersonResponse.Address):
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
        if (string.IsNullOrWhiteSpace(sortBy))
            return allPersons;

        List<PersonResponse> sortedPersonList = (sortBy, sortOrder) switch
        {
            (nameof(PersonResponse.Name), SortOrderEnum.ASC) => allPersons.OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase).ToList(),
            (nameof(PersonResponse.Name), SortOrderEnum.DESC) => allPersons.OrderByDescending(p => p.Name, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Email), SortOrderEnum.ASC) => allPersons.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),
            (nameof(PersonResponse.Email), SortOrderEnum.DESC) => allPersons.OrderByDescending(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.DateOfBirth), SortOrderEnum.ASC) => allPersons.OrderBy(p => p.DateOfBirth).ToList(),
            (nameof(PersonResponse.DateOfBirth), SortOrderEnum.DESC) => allPersons.OrderByDescending(p => p.DateOfBirth).ToList(),

            (nameof(PersonResponse.Age), SortOrderEnum.ASC) => allPersons.OrderBy(p => p.Age).ToList(),
            (nameof(PersonResponse.Age), SortOrderEnum.DESC) => allPersons.OrderByDescending(p => p.Age).ToList(),

            (nameof(PersonResponse.Gender), SortOrderEnum.ASC) => allPersons.OrderBy(p => p.Gender).ToList(),
            (nameof(PersonResponse.Gender), SortOrderEnum.DESC) => allPersons.OrderByDescending(p => p.Gender).ToList(),

            (nameof(PersonResponse.CountryName), SortOrderEnum.ASC) => allPersons.OrderBy(p => p.CountryName).ToList(),
            (nameof(PersonResponse.CountryName), SortOrderEnum.DESC) => allPersons.OrderByDescending(p => p.CountryName).ToList(),

            (nameof(PersonResponse.Address), SortOrderEnum.ASC) => allPersons.OrderBy(p => p.Address).ToList(),
            (nameof(PersonResponse.Address), SortOrderEnum.DESC) => allPersons.OrderByDescending(p => p.Address).ToList(),

            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderEnum.ASC) => allPersons.OrderBy(p => p.ReceiveNewsLetters).ToList(),
            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderEnum.DESC) => allPersons.OrderByDescending(p => p.ReceiveNewsLetters).ToList(),

            _ => allPersons
        };

        return sortedPersonList;
    }

    public PersonResponse UpdatePerson(PersonUpdateRequest? requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);

        ValidationHelper.ModelValidation(requestModel);

        Person matchingPerson = _personDataStore.FirstOrDefault(p => p.Id == requestModel.Id);

        if (matchingPerson == null)
            throw new ArgumentException("Given id doesn't exist");

        // Notice this matching person is in list, also this is reference type
        // Updating here will also update the data store
        // Maybe the implementation will be different if we use database
        matchingPerson.Name = requestModel.Name;
        matchingPerson.Email = requestModel.Email;
        matchingPerson.DateOfBirth = requestModel.DateOfBirth;
        matchingPerson.Gender = requestModel.Gender.ToString();
        matchingPerson.CountryId = requestModel.CountryId;
        matchingPerson.Address = requestModel.Address;
        matchingPerson.ReceiveNewsLetters = requestModel.ReceiveNewsLetters;

        return ConvertPersonToPersonResponse(matchingPerson);
    }

    public bool DeletePerson(Guid? id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Person? person = _personDataStore.FirstOrDefault(p => p.Id == id);
        if (person == null)
            return false;

        _personDataStore.RemoveAll(p => p.Id == id);
        return true;
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
