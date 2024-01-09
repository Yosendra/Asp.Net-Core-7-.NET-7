using Microsoft.EntityFrameworkCore;

/*
 * We need to inherit DbContext class, but the class is in Entity Framework Core. So we need to install
 * the package from Nuget Package Manager
 */

namespace Entities;

public class PersonsDbContext : DbContext
{
    public DbSet<Country> Countries { get; set;}
    public DbSet<Person> Persons { get; set;}

    /* We need to bind the DbSet to the corresponding tables
     * In order to that, we need to override an existing virtual method OnModelCreating() in DbContext class
     */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Important part in using EFCore
        modelBuilder.Entity<Country>().ToTable("Countries");
        modelBuilder.Entity<Person>().ToTable("Persons");
    }
}
