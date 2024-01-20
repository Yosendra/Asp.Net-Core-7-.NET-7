using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountryService : ICountryService
{
    private readonly PersonsDbContext _db;

    public CountryService(PersonsDbContext personsDbContext)
    {
        _db = personsDbContext;
    }

    public async Task<CountryResponse> AddCountry(CountryAddRequest? requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel, nameof(requestModel));

        if (requestModel.Name == null)
        {
            string errorMessage = string.Format("{0} cannot be null.", nameof(requestModel.Name));
            throw new ArgumentException(errorMessage);
        }

        requestModel.Name = requestModel.Name.Trim();
        if (await _db.Countries.AnyAsync(c => c.Name!.ToLower() == requestModel.Name.ToLower()))
        {
            string errorMessage = string.Format("{0} country is already exist.", requestModel.Name);
            throw new ArgumentException(errorMessage);
        }

        Country country = requestModel.ToCountry();
        country.Id = Guid.NewGuid();
        _db.Countries.Add(country);
        await _db.SaveChangesAsync();

        return country.ToCountryResponse();
    }

    public async Task<List<CountryResponse>> GetAllCountries()
    {
        return await _db.Countries.Select(country => country.ToCountryResponse()).ToListAsync();
    }

    public async Task<CountryResponse?> GetCountryById(Guid? id)
    {
        if (id == null) return null;

        Country? country = await _db.Countries.FirstOrDefaultAsync(c => c.Id == id);

        return country?.ToCountryResponse();
    }

    public async Task<int> UploadCountryFromExcelFile(IFormFile formFile) // file from Browser
    {
        MemoryStream memoryStream = new();
        await formFile.CopyToAsync(memoryStream);
        int countryInserted = 0;

        using (ExcelPackage excelPackage = new(memoryStream))
        {
            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets["Country"];

            int rowCount = workSheet.Dimension.Rows;
            for (int row = 2; row <= rowCount; row++)   // row index in excel start from '1'. '1' used for header, '2' and so on is the actual data
            {
                string? cellValue = Convert.ToString(workSheet.Cells[row, 1].Value);
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    string? countryName = cellValue;
                    bool isCountryExist = await _db.Countries.AnyAsync(c => c.Name == countryName);
                    if (!isCountryExist)
                    {
                        Country country = new()
                        {
                            Id = Guid.NewGuid(),
                            Name = countryName,
                        };

                        _db.Countries.Add(country);
                        await _db.SaveChangesAsync();
                        
                        countryInserted++;
                    }
                }
            }
        }

        return countryInserted;
    }
}
