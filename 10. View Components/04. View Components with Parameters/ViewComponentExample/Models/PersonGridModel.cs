namespace ViewComponentExample.Models;

public class PersonGridModel
{
    public string Title { get; set; } = string.Empty;
    public List<Person> Persons { get; set; } = new();
}
