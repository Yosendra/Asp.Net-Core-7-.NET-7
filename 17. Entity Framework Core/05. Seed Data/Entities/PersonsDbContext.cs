using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class PersonsDbContext : DbContext
{
    public DbSet<Country> Countries { get; set;}
    public DbSet<Person> Persons { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>().ToTable("Countries");
        modelBuilder.Entity<Person>().ToTable("Persons");

        // Seed to Countries
        //modelBuilder.Entity<Country>().HasData(
        //    new Country() { Id = Guid.NewGuid(), Name = "Sample Country", }
        //);

        // We can also provide the data through json file
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
}
