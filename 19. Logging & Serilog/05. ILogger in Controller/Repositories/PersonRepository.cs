using System.Linq.Expressions;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;

namespace Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<PersonRepository> _logger;     // the generic type is the location where the logger being injected, in this case it is PersonRepository 

    public PersonRepository(ApplicationDbContext db, ILogger<PersonRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Person> AddPerson(Person person)
    {
        _db.Persons.Add(person);
        await _db.SaveChangesAsync();

        return person;
    }

    public async Task<bool> DeletePersonById(Guid id)
    {
        Person? person = await _db.Persons.SingleOrDefaultAsync(p => p.Id == id);
        if (person == null)
            return false;

        _db.Persons.Remove(person);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<List<Person>> GetAllPersons()
    {
        _logger.LogInformation("GetAllPersons of PersonRepository");

        return await _db.Persons
            .Include(p => p.Country)
            .ToListAsync();
    }

    public async Task<List<Person>> GetFilteredPerson(Expression<Func<Person, bool>> predicate)     // notice this is quite unique
    {
        _logger.LogInformation("GetFilteredPerson of PersonRepository");

        return await _db.Persons
            .Include(p => p.Country)
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<Person?> GetPersonById(Guid id)
    {
        return await _db.Persons
            .Include(p => p.Country)
            .SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Person?> UpdatePerson(Person person)
    {
        Person? matchingPerson = await _db.Persons.SingleOrDefaultAsync(p => p.Id == person.Id);
        if (matchingPerson == null)
            return person;

        matchingPerson.Name = person.Name;
        matchingPerson.Email = person.Email;
        matchingPerson.DateOfBirth = person.DateOfBirth;
        matchingPerson.Gender = person.Gender;
        matchingPerson.Address = person.Address;
        matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;
        matchingPerson.CountryId = person.CountryId;

        await _db.SaveChangesAsync();

        return matchingPerson;
    }
}
