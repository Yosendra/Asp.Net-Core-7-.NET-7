using System.ComponentModel.DataAnnotations;

namespace CityManager.WebApi.Models;

public class City
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "City name cannot be blank")]
    public string? Name { get; set; }
}
