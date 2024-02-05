using System.Drawing;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RepositoryContracts;
using Serilog;
using SerilogTimings;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helper;

namespace Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<PersonService> _logger;
    private readonly IDiagnosticContext _diagnosticContext;

    public PersonService(IPersonRepository personRepository, ILogger<PersonService> logger, IDiagnosticContext diagnosticContext)
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

    public async Task<PersonResponse?> GetPersonById(Guid? id)
    {
        if (id == null)
            return null;

        Person? person = await _personRepository.GetPersonById(id.Value);

        return person?.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetAllPersons()
    {
        return (await _personRepository.GetAllPersons())
            .ConvertAll(p => p.ToPersonResponse());
    }

    public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? keyword)
    {
        _logger.LogInformation("GetFilteredPersons of PersonService");

        keyword ??= string.Empty;
        List<Person> personList = new();

        using (Operation.Time("Time for GetFilteredPerson() in Database"))
        {
            personList = searchBy switch
            {
                nameof(PersonResponse.Name)
                    => await _personRepository.GetFilteredPerson(p => p.Name.Contains(keyword)),

                nameof(PersonResponse.Email)
                    => await _personRepository.GetFilteredPerson(p => p.Email.Contains(keyword)),

                nameof(PersonResponse.Gender)
                    => await _personRepository.GetFilteredPerson(p => p.Gender.Contains(keyword)),

                nameof(PersonResponse.CountryId)
                    => await _personRepository.GetFilteredPerson(p => p.Country.Name.Contains(keyword)),

                nameof(PersonResponse.Address)
                    => await _personRepository.GetFilteredPerson(p => p.Address.Contains(keyword)),

                nameof(PersonResponse.DateOfBirth)
                    => await _personRepository.GetFilteredPerson(p => p.DateOfBirth.Value.ToString("dd MMM yyyy").Contains(keyword)),

                _ => await _personRepository.GetAllPersons()
            };
        }

        _diagnosticContext.Set("Persons", personList);

        return personList.ConvertAll(p => p.ToPersonResponse());
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

    public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);

        ValidationHelper.ModelValidation(requestModel);

        Person? matchingPerson = await _personRepository.GetPersonById(requestModel.Id.Value);
        if (matchingPerson == null)
            throw new ArgumentException("Given id doesn't exist");

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

    public async Task<bool> DeletePerson(Guid? id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Person? person = await _personRepository.GetPersonById(id.Value);
        if (person == null)
            return false;

        await _personRepository.DeletePersonById(person.Id!.Value);

        return true;
    }

    public async Task<MemoryStream> GetPersonCSV()
    {
        MemoryStream memoryStream = new();
        StreamWriter streamWriter = new(memoryStream);
        
        CsvConfiguration csvConfiguration = new(CultureInfo.InvariantCulture);
        CsvWriter csvWriter = new(streamWriter, csvConfiguration);

        csvWriter.WriteField(nameof(PersonResponse.Name));
        csvWriter.WriteField(nameof(PersonResponse.Email));
        csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
        csvWriter.WriteField(nameof(PersonResponse.Age));
        csvWriter.WriteField(nameof(PersonResponse.Gender));
        csvWriter.WriteField(nameof(PersonResponse.Address));
        csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
        csvWriter.WriteField(nameof(PersonResponse.CountryName));
        csvWriter.NextRecord();

        var persons = (await _personRepository.GetAllPersons())
            .ConvertAll(p => p.ToPersonResponse());

        foreach (var person in persons)
        {
            csvWriter.WriteField(person.Name);
            csvWriter.WriteField(person.Email);
            csvWriter.WriteField(person.DateOfBirth.HasValue
                ? person.DateOfBirth.Value.ToString("yyyy-MM-dd")
                : "");
            csvWriter.WriteField(person.Age);
            csvWriter.WriteField(person.Gender);
            csvWriter.WriteField(person.Address);
            csvWriter.WriteField(person.ReceiveNewsLetters);
            csvWriter.WriteField(person.CountryName);
            csvWriter.NextRecord();
            csvWriter.Flush();
        }

        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task<MemoryStream> GetPersonExcel()
    {
        MemoryStream memoryStream = new();
        using (ExcelPackage excelPackage = new(memoryStream))
        {
            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonSheet");
            workSheet.Cells["A1"].Value = "Name";
            workSheet.Cells["B1"].Value = "Email";
            workSheet.Cells["C1"].Value = "DateOfBirth";
            workSheet.Cells["D1"].Value = "Age";
            workSheet.Cells["E1"].Value = "Gender";
            workSheet.Cells["F1"].Value = "Address";
            workSheet.Cells["G1"].Value = "ReceiveNewsLetters";
            workSheet.Cells["H1"].Value = "CountryName";

            using (ExcelRange headerCells = workSheet.Cells["A1:H1"])
            {
                headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                headerCells.Style.Font.Bold = true;
            }

            int row = 2;
            var persons = (await _personRepository.GetAllPersons())
                .ConvertAll(p => p.ToPersonResponse());

            foreach (var person in persons)
            {
                workSheet.Cells[row, 1].Value = person.Name;
                workSheet.Cells[row, 2].Value = person.Email;
                workSheet.Cells[row, 3].Value = person.DateOfBirth.HasValue
                    ? person.DateOfBirth.Value.ToString("yyyy-MM-dd")
                    : "";
                workSheet.Cells[row, 4].Value = person.Age;
                workSheet.Cells[row, 5].Value = person.Gender;
                workSheet.Cells[row, 6].Value = person.Address;
                workSheet.Cells[row, 7].Value = person.ReceiveNewsLetters;
                workSheet.Cells[row, 8].Value = person.CountryName;

                row++;
            }

            workSheet.Cells[$"A1:H{row}"].AutoFitColumns();
            await excelPackage.SaveAsync();
        }

        memoryStream.Position = 0;
        return memoryStream;
    }
}
