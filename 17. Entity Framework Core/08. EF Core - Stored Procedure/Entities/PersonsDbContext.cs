using System.Text.Json;
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
        // this is the command to execute the SP
        return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
    }
}
