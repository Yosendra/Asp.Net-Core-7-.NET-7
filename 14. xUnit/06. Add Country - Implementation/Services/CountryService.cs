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
        // 1. Check if "countryAddRequest" is not null.
        // Request cannot be null
        ArgumentNullException.ThrowIfNull(countryAddRequest, nameof(countryAddRequest));

        // 2. Validate all properties of "countryAddRequest"
        // One of required property cannot be invalid
        if (countryAddRequest.CountryName == null)
        {
            string errorMessage = string.Format("{0} cannot be null.", nameof(countryAddRequest.CountryName));
            throw new ArgumentException(errorMessage);
        }

        // 3. CountryName cannot be duplicate
        if (_countries.Any(c => c.Name!.ToLower() == countryAddRequest.CountryName.ToLower()))
        {
            string errorMessage = string.Format("{0} country is already exist.", countryAddRequest.CountryName);
            throw new ArgumentException(errorMessage);
        }

        // 4. Convert "countryAddRequest" to "Country"
        Country country = countryAddRequest.ToCountry();

        // 5. Generate a new Id for "Country"
        country.Id = Guid.NewGuid();

        // 6. Add "Country" to the list
        _countries.Add(country);

        // 7. Return "CountryResponse" object with Country's generated Id before
        return country.ToCountryResponse();
    }
}
