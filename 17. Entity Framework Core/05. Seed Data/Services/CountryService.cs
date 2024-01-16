using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountryService : ICountryService
{
    private readonly List<Country> _countryDataStore;

    public CountryService(bool initialize = true) // Give 'false' at unit test 
    {
        _countryDataStore = new();
        
        if (initialize)
        {
            _countryDataStore.AddRange(new List<Country>()
            {
                new Country() { Id = Guid.Parse("D97ADA24-50A2-4F5D-B88D-7588F40F0351"), Name = "America" },
                new Country() { Id = Guid.Parse("C7FA54B0-B8BD-4B78-B151-738961031B06"), Name = "Germany" },
                new Country() { Id = Guid.Parse("BF61660F-6320-4C95-A752-DF655D29A2DE"), Name = "Argentina" },
                new Country() { Id = Guid.Parse("AD54C2C6-088E-4393-B87C-F30C04CFF344"), Name = "France" },
                new Country() { Id = Guid.Parse("F6B32853-5BFC-4655-A7EA-A86DBACE8DDB"), Name = "Brazil" },
            });
        }
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
