namespace Entities;

// Inside the class, declare all the properties that you want to store in database storage
// This class is called 'Domain Model'
// We cannot expose domain model to the presentation layer (controller)
// That is why we are using DTO 

/// <summary>
/// Domain Model for Country
/// </summary>
public class Country
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
