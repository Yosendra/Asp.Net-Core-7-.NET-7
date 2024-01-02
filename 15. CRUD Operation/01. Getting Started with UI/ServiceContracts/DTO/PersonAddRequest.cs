using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

/// <summary>
/// Acts as a DTO for inserting a new person
/// </summary>
public class PersonAddRequest
{
    [Required(ErrorMessage = "Person name cannot be blank")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Email cannot be blank")]
    [EmailAddress(ErrorMessage = "Email should be in a valid email format")]
    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }
    public GenderOption? Gender { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }
    public Guid? CountryId { get; set; }

    /// <summary>
    /// Convert the current object of PersonAddRequest into a new object of Person type
    /// </summary>
    /// <returns></returns>
    public Person ToPerson()
    {
        return new()
        {
             Name = Name,
             Email = Email,
             DateOfBirth = DateOfBirth,
             Gender = Gender.ToString(),
             Address = Address,
             ReceiveNewsLetters = ReceiveNewsLetters,
             CountryId = CountryId,
        };
    }
}
