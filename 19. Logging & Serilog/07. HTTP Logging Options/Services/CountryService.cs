using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountryService : ICountryService
{
    private readonly ICountryRepository _countryRepository;

    public CountryService(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
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
        Country? country = await _countryRepository.GetCountryByName(requestModel.Name);
        if (country != null)
        {
            string errorMessage = string.Format("{0} country is already exist.", requestModel.Name);
            throw new ArgumentException(errorMessage);
        }

        country = requestModel.ToCountry();
        country.Id = Guid.NewGuid();
        await _countryRepository.AddCountry(country);

        return country.ToCountryResponse();
    }

    public async Task<List<CountryResponse>> GetAllCountries()
    {
        var countryList = await _countryRepository.GetAllCountries();

        return countryList.Select(country => country.ToCountryResponse()).ToList();
    }

    public async Task<CountryResponse?> GetCountryById(Guid? id)
    {
        if (id == null) return null;

        Country? country = await _countryRepository.GetCountryById(id.Value);

        return country?.ToCountryResponse();
    }

    public async Task<int> UploadCountryFromExcelFile(IFormFile formFile) // file from Browser
    {
        MemoryStream memoryStream = new();
        await formFile.CopyToAsync(memoryStream);

        using ExcelPackage excelPackage = new(memoryStream);
        ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets["Country"];

        int rowCount = workSheet.Dimension.Rows,
            countryInserted = 0;

        /* row index in excel start from '1'
         * '1' used for header
         * '2' and so on is the actual data
         */
        for (int row = 2; row <= rowCount; row++)
        {
            string? cellValue = Convert.ToString(workSheet.Cells[row, 1].Value);
            if (!string.IsNullOrWhiteSpace(cellValue))
            {
                string countryName = cellValue;
                Country? country = await _countryRepository.GetCountryByName(countryName);
                bool isCountryExist = country != null;
                if (!isCountryExist)
                {
                    country = new()
                    {
                        Id = Guid.NewGuid(),
                        Name = countryName,
                    };
                    await _countryRepository.AddCountry(country);
                    
                    countryInserted++;
                }
            }
        }

        return countryInserted;
    }
}
