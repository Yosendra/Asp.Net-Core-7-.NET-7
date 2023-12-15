namespace ModelValidationExample.Models;

using System.ComponentModel.DataAnnotations;

public class Person
{
    // Process
    // Request -> Model Binding -> Model Validation
    //  -> Controller (receive request data as model object,
    //                  send response data as model object)
    //  -> Execute Action Result
    //  -> Response

    // Here in model class, we put the validation attribute above the property we want to validate
    [Required(ErrorMessage = "Name can't be empty or null")]    // pass argument to attribute to give custom message
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
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
