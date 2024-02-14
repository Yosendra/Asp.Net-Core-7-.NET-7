using CityManager.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CityManager.WebApi.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public ApplicationDbContext()
    {
    }

    public virtual DbSet<City> City { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        List<City> cityList = new()
        {
            new(){ Id = Guid.Parse("5A727619-7A4B-4EA8-BD63-28A817153020"), Name = "Jakarta" },
            new(){ Id = Guid.Parse("614C2DEA-4904-472D-8E23-6B27601FF0D8"), Name = "Canberra" },
            new(){ Id = Guid.Parse("97819A90-B6BA-4974-BC76-69185A0B0CD0"), Name = "Ankara" },
        };
        modelBuilder.Entity<City>().HasData(cityList);
    }
}
