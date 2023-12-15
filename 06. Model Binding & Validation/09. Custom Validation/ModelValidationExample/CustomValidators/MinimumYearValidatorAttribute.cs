using System.ComponentModel.DataAnnotations;

namespace ModelValidationExample.CustomValidators;

public class MinimumYearValidatorAttribute : ValidationAttribute    // must inherit from this class
{
    public int MinimumYear { get; set; } = 2000;
    public string DefaultErrorMessage { get; set; } = "Year should less than {0}";

    public MinimumYearValidatorAttribute()
    {
        
    }

    // Create parameterized constructor to enable passing argument in the attribute
    public MinimumYearValidatorAttribute(int minimumYear)
    {
        MinimumYear = minimumYear;
    }

    // override this method
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not null)
        {
            DateTime date = (DateTime)value;
            if (date.Year > MinimumYear)
                // ErrorMessage is from the parent
                // If attribute do not provide 'minimumYear' argument, use our custom 'DefaultErrorMessage' message
                return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, MinimumYear));
            else
                return ValidationResult.Success;
        }

        return null; // no validation
    }
}
