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

    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    {
        ArgumentNullException.ThrowIfNull(countryAddRequest, nameof(countryAddRequest));

        if (countryAddRequest.CountryName == null)
        {
            string errorMessage = string.Format("{0} cannot be null.", nameof(countryAddRequest.CountryName));
            throw new ArgumentException(errorMessage);
        }

        if (_countries.Any(c => c.Name!.ToLower() == countryAddRequest.CountryName.ToLower()))
        {
            string errorMessage = string.Format("{0} country is already exist.", countryAddRequest.CountryName);
            throw new ArgumentException(errorMessage);
        }

        Country country = countryAddRequest.ToCountry();
        country.Id = Guid.NewGuid();
        _countries.Add(country);

        return country.ToCountryResponse();
    }

    public List<CountryResponse> GetAllCountries()
    {
        return _countries.Select(country => country.ToCountryResponse())
                         .ToList();
    }

    public CountryResponse? GetCountryById(Guid? id)
    {
        // Check if Id is not null
        if (id == null)
            return null;

        // Get matching country from List<Country> based on Id
        Country? country = _countries.FirstOrDefault(c => c.Id == id);

        // Convert matching country object to CountryResponse object
        // Return CountryResponse object
        // Return null if country not found
        return country?.ToCountryResponse();
    }
}
