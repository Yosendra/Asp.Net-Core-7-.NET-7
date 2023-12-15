namespace ModelValidationExample.Models;

// Model Validation Attributes
//  [Required(ErrorMessage = "your message")]
//    Specifies that the property value is required (can't be blank or empty)
//
//  [StringLength(int maximumLength, MinimumLength = value, ErrorMessage = "value")]
//    Specifies minimum and maximum length (number of characters) allowed in the string.
//
//  [Range(int minimum, int maximum, ErrorMessage = "value")]
//    Specifies minimum and maximum numerical value allowed.

using System.ComponentModel.DataAnnotations;

public class Person
{
    [Required(ErrorMessage = "{0} can't be empty or null")] // {0} -> Property name, which is 'Name'
    [Display(Name = "Person Name")] // This attribute is to custom property displayed in error message response
    [StringLength(40, MinimumLength = 3, ErrorMessage = "{0} should be between {2} and {1} characters long")]
    public string? Name { get; set; }

    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }

    [Range(0, 999.99, ErrorMessage = "{0} should be between {1} and {2}")]
    public double? Price { get; set; }

    public override string ToString()
    {
        return $"Person object\n" +
               $"Name: {Name}\n" +
               $"Email: {Email}\n" +
               $"Phone: {Phone}\n" +
               $"Password: {Password}\n" +
               $"Confirm Password: {ConfirmPassword}\n" +
               $"Price: {Price}";
    }
}
