using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountryService : ICountryService
{
    private readonly List<Country> _countries;

    public CountryService()
    {
        _countries = new List<Country>();
    }

    public CountryResponse AddCountry(CountryAddRequest? requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel, nameof(requestModel));

        if (requestModel.CountryName == null)
        {
            string errorMessage = string.Format("{0} cannot be null.", nameof(requestModel.CountryName));
            throw new ArgumentException(errorMessage);
        }

        requestModel.CountryName = requestModel.CountryName.Trim();
        if (_countries.Any(c => c.Name!.ToLower() == requestModel.CountryName.ToLower()))
        {
            string errorMessage = string.Format("{0} country is already exist.", requestModel.CountryName);
            throw new ArgumentException(errorMessage);
        }

        Country country = requestModel.ToCountry();
        country.Id = Guid.NewGuid();
        _countries.Add(country);

        return country.ToCountryResponse();
    }

    public List<CountryResponse> GetAllCountries()
    {
        return _countries.Select(country => country.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryById(Guid? id)
    {
        if (id == null) return null;

        Country? country = _countries.FirstOrDefault(c => c.Id == id);

        return country?.ToCountryResponse();
    }
}
