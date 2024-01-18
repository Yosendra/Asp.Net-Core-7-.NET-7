using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class PersonsDbContext : DbContext
{
    public DbSet<Country> Countries { get; set;}
    public DbSet<Person> Persons { get; set;}

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

        // Fluent API
        modelBuilder.Entity<Person>().Property(c => c.TIN)
            .HasColumnName("TaxIdentificationNumber")
            .HasColumnType("varchar(8)")
            .HasDefaultValue("ABC12345");

        modelBuilder.Entity<Person>()
            .HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8");

        // Table Relation
        //modelBuilder.Entity<Person>(entity =>
        //{
        //    entity
        //        .HasOne(p => p.Country)
        //        .WithMany(c => c.Persons)
        //        .HasForeignKey(p => p.CountryId);
        //});

        // Not common practice to define relation in this modelBuilder, instead we use attribute on the entity class
    }

    #region Stored Procedure
    public List<Person> sp_GetAllPersons()
        => Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();

    public int sp_InsertPerson(Person person)
    {
        SqlParameter[] parameters =
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
            "@Id, @Name, @Email, @DateOfBirth, @Gender, " +
            "@Address, @ReceiveNewsLetters, @CountryId",
            parameters);
    }
    #endregion
}
