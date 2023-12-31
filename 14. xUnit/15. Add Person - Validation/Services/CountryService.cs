using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountryService : ICountryService
{
    private readonly List<Country> _countryDataStore;

    public CountryService()
    {
        _countryDataStore = new();
    }

    public CountryResponse AddCountry(CountryAddRequest? requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel, nameof(requestModel));

        if (requestModel.Name == null)
        {
            string errorMessage = string.Format("{0} cannot be null.", nameof(requestModel.Name));
            throw new ArgumentException(errorMessage);
        }

        requestModel.Name = requestModel.Name.Trim();
        if (_countryDataStore.Any(c => c.Name!.ToLower() == requestModel.Name.ToLower()))
        {
            string errorMessage = string.Format("{0} country is already exist.", requestModel.Name);
            throw new ArgumentException(errorMessage);
        }

        Country country = requestModel.ToCountry();
        country.Id = Guid.NewGuid();
        _countryDataStore.Add(country);

        return country.ToCountryResponse();
    }

    public List<CountryResponse> GetAllCountries()
    {
        return _countryDataStore.Select(country => country.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryById(Guid? id)
    {
        if (id == null) return null;

        Country? country = _countryDataStore.FirstOrDefault(c => c.Id == id);

        return country?.ToCountryResponse();
    }
}
