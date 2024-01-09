using System.ComponentModel.DataAnnotations;

namespace Entities;

/// <summary>
/// Person domain model class
/// </summary>
public class Person
{
    [Key] // notice this
    public Guid? Id { get; set; }

    [StringLength(40)]
    public string? Name { get; set; }

    [StringLength(40)]
    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [StringLength(10)]
    public string? Gender { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    public bool ReceiveNewsLetters { get; set; }
    
    // This is a Foreign Key
    public Guid? CountryId { get; set; }
}
