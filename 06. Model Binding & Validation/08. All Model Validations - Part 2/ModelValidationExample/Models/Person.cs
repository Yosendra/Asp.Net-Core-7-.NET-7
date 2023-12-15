using System.Reflection;

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
//
//  [RegularExpression(string pattern, ErrorMessage = "value")]
//    Specifies the valid pattern(regular expression).
//
//  [EmailAddress(ErrorMessage = "value")]
//    Specifies that the value should be a valid email address.
//
//  [Phone(ErrorMessage = "value")]
//    Specifies that the value should be a valid phone number).
//    Eg: (999)-999-9999 or 9876543210
//
//  [Compare(string otherProperty, ErrorMessage = "value")]
//    Specifies that the values of current property and other property should be same.
//
//  [Url(ErrorMessage = "value")]
//    Specifies that the value should be a valid url(website address).
//    Eg: http://www.example.com
//
//  [ValidateNever]
//    Specifies that the property should not be validated(excludes the property from model validation).

using System.ComponentModel.DataAnnotations;

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
