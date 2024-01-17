using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class PersonsDbContext : DbContext
{
    public DbSet<Country> Countries { get; set;}
    public DbSet<Person> Persons { get; set;}

    // Contructor
    public PersonsDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>().ToTable("Countries");
        modelBuilder.Entity<Person>().ToTable("Persons");

        // Seed to Countries
        string countriesJson = File.ReadAllText("countries.json");
        var listOfCountries = JsonSerializer.Deserialize<List<Country>>(countriesJson);
        foreach (var country in listOfCountries)
            modelBuilder.Entity<Country>().HasData(country);

        // Seed to Persons
        string personsJson = File.ReadAllText("persons.json");
        var listOfPersons = JsonSerializer.Deserialize<List<Person>>(personsJson);
        foreach (var person in listOfPersons)
            modelBuilder.Entity<Person>().HasData(person);
    }

    public List<Person> sp_GetAllPersons()
    {
        return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
    }

    public int sp_InsertPerson(Person person)
    {
        var parameters = new SqlParameter[]
        {
            new("@Id", person.Id),
            new("@Name", person.Name),
            new("@Email", person.Email),
            new("@DateOfBirth", person.DateOfBirth),
            new("@Gender", person.Gender),
            new("@Address", person.Address),
            new("@ReceiveNewsLetters", person.ReceiveNewsLetters),
            new("@CountryId", person.CountryId),
        };

        return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] " +
            "@Id, " +
            "@Name, " +
            "@Email, " +
            "@DateOfBirth, " +
            "@Gender, " +
            "@Address, " +
            "@ReceiveNewsLetters, " +
            "@CountryId", 
            parameters);
    }
}
