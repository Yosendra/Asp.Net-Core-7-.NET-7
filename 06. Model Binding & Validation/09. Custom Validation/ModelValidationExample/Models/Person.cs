namespace ModelValidationExample.Models;

// Custom Validation Attributes
//  look at 'MinimumYearValidatorAttribute.cs'

using System.ComponentModel.DataAnnotations;
using ModelValidationExample.CustomValidators;

public class Person
{
    [Required(ErrorMessage = "{0} can't be empty or null")] // {0} -> Property name, which is 'Name'
    [Display(Name = "Person Name")] // This attribute is to custom property displayed in error message response
    [StringLength(40, MinimumLength = 3, ErrorMessage = "{0} should be between {2} and {1} characters long")]
    [RegularExpression("^[A-Za-z .]$", ErrorMessage = "{0} should contain only alphabets, space, and dot (.)")]
    public string? Name { get; set; }


    [EmailAddress(ErrorMessage = "{0} should be a proper email address")]
    [Required(ErrorMessage = "{0} can't be blank")]
    public string? Email { get; set; }


    [Phone(ErrorMessage = "{0} should contain 10 digits")]
    public string? Phone { get; set; }


    [Required(ErrorMessage = "{0} can't be blank")]
    public string? Password { get; set; }


    [Required(ErrorMessage = "{0} can't be blank")]
    [Compare("Password", ErrorMessage = "{0} and {1} do not match")]
    [Display(Name = "Re-enter Password")]
    public string? ConfirmPassword { get; set; }


    [Range(0, 999.99, ErrorMessage = "{0} should be between {1} and {2}")]
    public double? Price { get; set; }


    [MinimumYearValidator(2002, ErrorMessage = "Maximum date of birth year is {0}")]
    public DateTime? DateOfBirth { get; set; }

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
