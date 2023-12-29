using Entities;

namespace ServiceContracts.DTO;

/// <summary>
/// DTO class that is used as return type for most of CountryService methods
/// </summary>
public class CountryResponse
{
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }
}

// Extension method
public static class CountryExtension 
{
    public static CountryResponse ToCountryResponse(this Country country)
    {
        return new()
        {
            CountryId = country.Id,
            CountryName = country.Name,
        };
    }
}

