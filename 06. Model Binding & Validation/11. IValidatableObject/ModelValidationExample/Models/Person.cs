using System.ComponentModel.DataAnnotations;
using ModelValidationExample.CustomValidators;

// IValidatableObject
//  Instead of writing custom validator as a seperate class,
//  you can write the validation code in the same model class
//  You don't want it to be reusable. Because if you want to make it reusable
//  it will be a bit complex because we have to use reflection.
//  We are not interested to make it reusable and need it fast

//  Case: either date of birth or age can be submitted, not both at the time  

namespace ModelValidationExample.Models;

public class Person : IValidatableObject    // implement IValidatableObject interface, look at the implemented method
{
    [Required(ErrorMessage = "{0} can't be empty or null")]
    [Display(Name = "Person Name")]
    [StringLength(40, MinimumLength = 3, ErrorMessage = "{0} should be between {2} and {1} characters long")]
    [RegularExpression("^[A-Za-z .]*$", ErrorMessage = "{0} should contain only alphabets, space, and dot (.)")]
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

    public DateTime? FromDate { get; set; }

    [DateRangeValidator("FromDate", ErrorMessage = "'FromDate' should be older than or equal to 'ToDate'")]
    public DateTime? ToDate { get; set; }

    public int? Age { get; set; }

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

    // 'Validate()' will be executed after model binding, before executing the action method
    //  Notice this method will only be execute after all validation
    //  in our attribute in this model is clear with no error,
    //  only then this validation evaluated

    // Supply all requested value correctly, also supply both Date of Birth and Age
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Your validation code here
        if (DateOfBirth.HasValue && Age.HasValue)
        {
            yield return new ValidationResult("Either of Date of Birth or Age must be supplied, not both", new[] { nameof(Age) });
        }
    }
}
