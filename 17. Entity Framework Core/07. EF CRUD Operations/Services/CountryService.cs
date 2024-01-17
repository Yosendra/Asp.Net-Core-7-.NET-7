using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountryService : ICountryService
{
    private readonly PersonsDbContext _db;

    // Inject out custom DbContext in constructor, we also remove our custom in-memory country data-store
    public CountryService(PersonsDbContext personsDbContext)
    {
        _db = personsDbContext;
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
        if (_db.Countries.Any(c => c.Name!.ToLower() == requestModel.Name.ToLower()))
        {
            string errorMessage = string.Format("{0} country is already exist.", requestModel.Name);
            throw new ArgumentException(errorMessage);
        }

        Country country = requestModel.ToCountry();
        country.Id = Guid.NewGuid();
        _db.Countries.Add(country);

        // Important part! to actually save the data in database, we have to run this command
        _db.SaveChanges();

        return country.ToCountryResponse();
    }

    public List<CountryResponse> GetAllCountries()
    {
        // Access the country table through "_db" field
        return _db.Countries.Select(country => country.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryById(Guid? id)
    {
        if (id == null) return null;

        // Access the country table through "_db" field
        Country? country = _db.Countries.FirstOrDefault(c => c.Id == id);

        return country?.ToCountryResponse();
    }
}
