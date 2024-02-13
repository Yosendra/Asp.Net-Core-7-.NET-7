using System.ComponentModel.DataAnnotations;

namespace CityManager.WebApi.Models;

public class City
{
    [Key]
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
