using Entities;

namespace RepositoryContracts;

/// <summary>
/// Represents data access logic for managin Country entity
/// </summary>
public interface ICountryRepository
{
    /// <summary>
    /// Add a new country object to the data store
    /// </summary>
    /// <param name="country">Country object to add</param>
    /// <returns>Returns the country object after adding it to the data store</returns>
    Task<Country> AddCountry(Country country);

    /// <summary>
    /// Return all countries in the data store
    /// </summary>
    /// <returns>All countries from the table</returns>
    Task<List<Country>> GetAllCountries();

    /// <summary>
    /// Returns a country object based on the given id
    /// </summary>
    /// <param name="id">Id to search</param>
    /// <returns>Matching country or null</returns>
    Task<Country?> GetCountryById(Guid id);

    /// <summary>
    /// Returns a country object based on the given name
    /// </summary>
    /// <param name="name">Country name to search</param>
    /// <returns>Matching country or null</returns>
    Task<Country?> GetCountryByName(string name);
}
