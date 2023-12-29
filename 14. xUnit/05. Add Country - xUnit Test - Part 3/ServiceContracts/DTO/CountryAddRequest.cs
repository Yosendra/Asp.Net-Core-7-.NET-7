using Entities;

namespace ServiceContracts.DTO;

/// <summary>
/// DTO class for adding a new country
/// </summary>
public class CountryAddRequest
{
    // As part of the service class, after validating this 'Name' property
    // If it is correct, we have to convert the same into 'Country' object
    // in order to add the same into the actual data source
    public string? CountryName { get; set; }
    public Country ToCountry()
    {
        return new()
        {
            Name = CountryName
        };
    }
}
