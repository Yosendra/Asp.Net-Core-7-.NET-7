using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly ApplicationDbContext _db;

    public CountryRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Country> AddCountry(Country country)
    {
        _db.Countries.Add(country);
        await _db.SaveChangesAsync();
        
        return country;
    }

    public async Task<List<Country>> GetAllCountries()
    {
        return await _db.Countries.ToListAsync();
    }

    public async Task<Country?> GetCountryById(Guid id)
    {
        return await _db.Countries.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Country?> GetCountryByName(string name)
    {
        return await _db.Countries.FirstOrDefaultAsync(c => c.Name == name);
    }
}
