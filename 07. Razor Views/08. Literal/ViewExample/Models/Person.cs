namespace ViewExample.Models;

public enum PersonGender
{
    Male, Female
}

public class Person
{
    public string? Name { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public PersonGender Gender { get; set; }
}
