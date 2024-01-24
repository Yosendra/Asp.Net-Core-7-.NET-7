using System.ComponentModel.DataAnnotations;

namespace Services.Helper;

public static class ValidationHelper
{
    public static void ModelValidation(object model)
    {
        ValidationContext validationContext = new(model);
        List<ValidationResult> validationResults = new();
        bool isRequestModelValid = Validator.TryValidateObject(model, validationContext, validationResults, true);
        if (!isRequestModelValid)
        {
            string? errorMessage = validationResults.FirstOrDefault()?.ErrorMessage;
            throw new ArgumentException(errorMessage);
        }
    }
}
