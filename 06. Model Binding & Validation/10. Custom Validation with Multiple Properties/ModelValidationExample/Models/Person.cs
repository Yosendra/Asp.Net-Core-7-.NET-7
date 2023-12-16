using System.ComponentModel.DataAnnotations;
using ModelValidationExample.CustomValidators;

// Custom Validation Attributes
//  look at 'FromDate' and 'ToDate' property
//  look at 'DateRangeValidatorAttribute.cs'

namespace ModelValidationExample.Models;

public class Person
{
    [Required(ErrorMessage = "{0} can't be empty or null")]
    [Display(Name = "Person Name")]
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

    // Multiple property validation
    public DateTime? FromDate { get; set; }

    [DateRangeValidator("FromDate", ErrorMessage = "'FromDate' should be older than or equal to 'ToDate'")]
    public DateTime? ToDate { get; set; }


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
