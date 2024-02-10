using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.Core.Dto;

public class RegisterDto
{
    [Required(ErrorMessage = "Name can't be blank")]
    public string PersonName { get; set; }

    [Required(ErrorMessage = "Email can't be blank")]
    [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
    [Remote("IsEmailAlreadyRegistered", "Account", ErrorMessage = "Email is already used")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Phone can't be blank")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number should contain numbers only")]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Password can't be blank")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password can't be blank")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
    public string ConfirmPassword { get; set; }
}
