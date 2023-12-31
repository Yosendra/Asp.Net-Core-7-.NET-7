using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

/// <summary>
/// Acts as a DTO for inserting a new person
/// </summary>
public class PersonAddRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public GenderOption? Gender { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }

    public Guid? CountryId { get; set; }
}
