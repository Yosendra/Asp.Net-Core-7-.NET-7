using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using ServiceContracts;
using ServiceContracts.DTO;
using RepositoryContracts;

namespace Services;

public class PersonGetterServiceWithFewExcelFields : IPersonGetterService
{
    private readonly PersonGetterService _personGetterService;
    private readonly IPersonRepository _personRepository;

    public PersonGetterServiceWithFewExcelFields(PersonGetterService personGetterService, IPersonRepository peersonRepository)
    {
        _personGetterService = personGetterService;
        _personRepository = peersonRepository;
    }

    public async Task<List<PersonResponse>> GetAllPersons()
    {
        return await _personGetterService.GetAllPersons();
    }

    public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? keyword)
    {
        return await _personGetterService.GetFilteredPersons(searchBy, keyword);
    }

    public async Task<PersonResponse?> GetPersonById(Guid? id)
    {
        return await _personGetterService.GetPersonById(id);
    }

    public async Task<MemoryStream> GetPersonCSV()
    {
        return await _personGetterService.GetPersonCSV();
    }

    public async Task<MemoryStream> GetPersonExcel()
    {
        MemoryStream memoryStream = new();
        using (ExcelPackage excelPackage = new(memoryStream))
        {
            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonSheet");
            workSheet.Cells["A1"].Value = "Name";
            workSheet.Cells["B1"].Value = "Age";
            workSheet.Cells["C1"].Value = "Gender";

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
                workSheet.Cells[row, 2].Value = person.Age;
                workSheet.Cells[row, 3].Value = person.Gender;

                row++;
            }

            workSheet.Cells[$"A1:C{row}"].AutoFitColumns();
            await excelPackage.SaveAsync();
        }

        memoryStream.Position = 0;
        return memoryStream;
    }
}
