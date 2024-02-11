using System.ComponentModel.DataAnnotations;

namespace ContactManager.Core.Dto;

public class LoginDto
{
    [Required(ErrorMessage = "Email can't be blank")]
    [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password can't be blank")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}
