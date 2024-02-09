using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

/// <summary>
/// Represents DTO class that is used as return type of most methods of Person Service
/// </summary>
public class PersonResponse
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public double? Age { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }
    public Guid? CountryId { get; set; }
    public string? CountryName { get; set; }

    public PersonUpdateRequest ToPersonUpdateRequest()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            Email = Email,
            DateOfBirth = DateOfBirth,
            Gender = Enum.Parse<GenderOption>(Gender!, true),
            Address = Address,
            ReceiveNewsLetters = ReceiveNewsLetters,
            CountryId = CountryId,
        };
    }

    #region Override Method
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (obj.GetType() != typeof(PersonResponse))
            return false;

        var target = (PersonResponse)obj;
        return Id == target.Id
                && Name == target.Name
                && Email == target.Email
                && DateOfBirth == target.DateOfBirth
                && Age == target.Age
                && Gender == target.Gender
                && Address == target.Address
                && ReceiveNewsLetters == target.ReceiveNewsLetters
                && CountryId == target.CountryId
                && CountryName == target.CountryName;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    #endregion
}

public static class PersonExtension
{
    public static PersonResponse ToPersonResponse(this Person person)
    {
        return new()
        {
            Id = person.Id,
            Name = person.Name,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            Gender = person.Gender,
            Address = person.Address,
            ReceiveNewsLetters = person.ReceiveNewsLetters,
            CountryId = person.CountryId,
            Age = person.DateOfBirth != null
                ? Math.Round((DateTime.Now - person.DateOfBirth).Value.TotalDays / 365.25)
                : null,
            CountryName = person.Country?.Name
        };
    }
}
