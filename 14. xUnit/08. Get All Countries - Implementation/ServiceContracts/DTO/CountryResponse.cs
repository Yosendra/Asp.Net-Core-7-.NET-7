using Entities;

namespace ServiceContracts.DTO;

/// <summary>
/// DTO class that is used as return type for most of CountryService methods
/// </summary>
public class CountryResponse
{
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }

    // Override .Equals()
    public override bool Equals(object? obj)
    {
        if (obj == null) 
            return false;

        if (obj.GetType() != typeof(CountryResponse)) 
            return false;

        var target = (CountryResponse) obj;
        return this.CountryId == target.CountryId
                && this.CountryName == target.CountryName;
    }

    // This is optional, compiler suggest us to also override .GetHashCode() method when we try to override .Equals()
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
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

