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
        throw new NotImplementedException();
    }
}
