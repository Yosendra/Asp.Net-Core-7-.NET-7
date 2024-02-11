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

namespace Services;

public class PersonGetterService : IPersonGetterService
{
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<PersonGetterService> _logger;
    private readonly IDiagnosticContext _diagnosticContext;

    public PersonGetterService(
        IPersonRepository personRepository, 
        ILogger<PersonGetterService> logger, 
        IDiagnosticContext diagnosticContext)
    {
        _personRepository = personRepository;
        _logger = logger;
        _diagnosticContext = diagnosticContext;
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
