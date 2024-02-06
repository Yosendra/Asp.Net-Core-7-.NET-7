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
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [Required(ErrorMessage = "Please select the gender of person")]
    public GenderOption? Gender { get; set; }

    [Required(ErrorMessage = "Address cannot be blank")]
    public string? Address { get; set; }

    public bool ReceiveNewsLetters { get; set; }

    [Required(ErrorMessage = "Please select a country")]
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
