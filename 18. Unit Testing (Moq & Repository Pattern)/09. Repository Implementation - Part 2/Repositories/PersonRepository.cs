using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly ApplicationDbContext _db;

    public PersonRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Person> AddPerson(Person person)
    {
        _db.Persons.Add(person);
        await _db.SaveChangesAsync();

        return person;
    }

    public async Task<bool> DeletePersonById(Guid id)
    {
        Person? person = await _db.Persons.FirstOrDefaultAsync(p => p.Id == id);
        _db.Persons.Remove(person);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<List<Person>> GetAllPersons()
    {
        return await _db.Persons.Include("Country").ToListAsync();
    }

    public async Task<List<Person>> GetFilteredPerson(Expression<Func<Person, bool>> predicate)     // notice this is quite unique
    {
        return await _db.Persons.Include("Country").Where(predicate).ToListAsync();
    }

    public async Task<Person?> GetPersonById(Guid id)
    {
        return await _db.Persons.Include("Country").FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Person?> UpdatePerson(Person person)
    {
        Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(p => p.Id == person.Id);

        if (matchingPerson is null)
            return matchingPerson;

        matchingPerson.Name = person.Name;
        matchingPerson.Email = person.Email;
        matchingPerson.DateOfBirth = person.DateOfBirth;
        matchingPerson.Gender = person.Gender;
        matchingPerson.Address = person.Address;
        matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;
        matchingPerson.CountryId = person.CountryId;

        await _db.SaveChangesAsync();

        return person;
    }
}
