using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

namespace Entities;

/// <summary>
/// Domain Model for Country
/// </summary>
public class Country
{
    [Key]
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public virtual ICollection<Person>? Persons { get; set; }
}
