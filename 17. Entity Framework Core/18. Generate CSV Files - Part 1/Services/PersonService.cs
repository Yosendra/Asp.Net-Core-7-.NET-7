﻿using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helper;
using CsvHelper;
using System.Globalization;

namespace Services;

public class PersonService : IPersonService
{
    private readonly PersonsDbContext _db;

    public PersonService(PersonsDbContext personsDbContext)
    {
        _db = personsDbContext;
    }

    public async Task<PersonResponse> AddPerson(PersonAddRequest? requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);

        ValidationHelper.ModelValidation(requestModel);

        Person person = requestModel.ToPerson();
        person.Id = Guid.NewGuid();

        _db.Persons.Add(person);
        await _db.SaveChangesAsync();

        return person.ToPersonResponse();
    }

    public async Task<PersonResponse?> GetPersonById(Guid? id)
    {
        if (id == null)
            return null;

        Person? person = await _db.Persons.Include("Country").FirstOrDefaultAsync(p => p.Id == id);

        return person != null
            ? person.ToPersonResponse()
            : null;
    }

    public async Task<List<PersonResponse>> GetAllPersons()
    {
        var personList = await _db.Persons.Include("Country").ToListAsync();
        return personList.Select(p => p.ToPersonResponse()).ToList();
    }

    public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? keyword)
    {
        List<PersonResponse> allPersons = await GetAllPersons();
        List<PersonResponse> matchingPersons = allPersons;

        if (string.IsNullOrWhiteSpace(searchBy) || (!string.IsNullOrWhiteSpace(searchBy) && string.IsNullOrWhiteSpace(keyword))) 
            return matchingPersons;

        switch (searchBy)
        {
            case nameof(PersonResponse.Name):
                matchingPersons = allPersons.Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case nameof(PersonResponse.Email):
                matchingPersons = allPersons.Where(p => p.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case nameof(PersonResponse.DateOfBirth):
                matchingPersons = allPersons.Where(p => p.DateOfBirth.Value
                    .ToString("dd MMMM yyyy")
                    .Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case nameof(PersonResponse.Gender):
                matchingPersons = allPersons.Where(p => p.Gender.Equals(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case nameof(PersonResponse.CountryId):
                matchingPersons = allPersons.Where(p => p.CountryName.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case nameof(PersonResponse.Address):
                matchingPersons = allPersons.Where(p => string.IsNullOrWhiteSpace(p.Address)
                    ? false
                    : p.Address.Contains(keyword!, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            default:
                matchingPersons = allPersons;
                break;
        }

        return matchingPersons;
    }

    public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderEnum sortOrder)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return allPersons;

        List<PersonResponse> sortedPersonList = (sortBy, sortOrder) switch
        {
            (nameof(PersonResponse.Name), SortOrderEnum.ASC)
                => allPersons.OrderBy(p => p.Name).ToList(),
            (nameof(PersonResponse.Name), SortOrderEnum.DESC)
                => allPersons.OrderByDescending(p => p.Name).ToList(),

            (nameof(PersonResponse.Email), SortOrderEnum.ASC)
                => allPersons.OrderBy(p => p.Email).ToList(),
            (nameof(PersonResponse.Email), SortOrderEnum.DESC)
                => allPersons.OrderByDescending(p => p.Email).ToList(),

            (nameof(PersonResponse.DateOfBirth), SortOrderEnum.ASC)
                => allPersons.OrderBy(p => p.DateOfBirth).ToList(),
            (nameof(PersonResponse.DateOfBirth), SortOrderEnum.DESC)
                => allPersons.OrderByDescending(p => p.DateOfBirth).ToList(),

            (nameof(PersonResponse.Age), SortOrderEnum.ASC)
                => allPersons.OrderBy(p => p.Age).ToList(),
            (nameof(PersonResponse.Age), SortOrderEnum.DESC)
                => allPersons.OrderByDescending(p => p.Age).ToList(),

            (nameof(PersonResponse.Gender), SortOrderEnum.ASC)
                => allPersons.OrderBy(p => p.Gender).ToList(),
            (nameof(PersonResponse.Gender), SortOrderEnum.DESC)
                => allPersons.OrderByDescending(p => p.Gender).ToList(),

            (nameof(PersonResponse.CountryName), SortOrderEnum.ASC)
                => allPersons.OrderBy(p => p.CountryName).ToList(),
            (nameof(PersonResponse.CountryName), SortOrderEnum.DESC)
                => allPersons.OrderByDescending(p => p.CountryName).ToList(),

            (nameof(PersonResponse.Address), SortOrderEnum.ASC)
                => allPersons.OrderBy(p => p.Address).ToList(),
            (nameof(PersonResponse.Address), SortOrderEnum.DESC)
                => allPersons.OrderByDescending(p => p.Address).ToList(),

            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderEnum.ASC)
                => allPersons.OrderBy(p => p.ReceiveNewsLetters).ToList(),
            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderEnum.DESC)
                => allPersons.OrderByDescending(p => p.ReceiveNewsLetters).ToList(),

            _ => allPersons
        };

        return sortedPersonList;
    }

    public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);

        ValidationHelper.ModelValidation(requestModel);

        Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(p => p.Id == requestModel.Id);

        if (matchingPerson == null)
            throw new ArgumentException("Given id doesn't exist");

        matchingPerson.Name = requestModel.Name;
        matchingPerson.Email = requestModel.Email;
        matchingPerson.DateOfBirth = requestModel.DateOfBirth;
        matchingPerson.Gender = requestModel.Gender.ToString();
        matchingPerson.CountryId = requestModel.CountryId;
        matchingPerson.Address = requestModel.Address;
        matchingPerson.ReceiveNewsLetters = requestModel.ReceiveNewsLetters;
        await _db.SaveChangesAsync();

        return matchingPerson.ToPersonResponse();
    }

    public async Task<bool> DeletePerson(Guid? id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Person? person = await _db.Persons.FirstOrDefaultAsync(p => p.Id == id);
        if (person == null)
            return false;

        _db.Persons.Remove(person);
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<MemoryStream> GetPersonCSV()
    {
        MemoryStream memoryStream = new();
        StreamWriter streamWriter = new(memoryStream);
        CsvWriter csvWriter = new(streamWriter, CultureInfo.InvariantCulture, true);

        csvWriter.WriteHeader<PersonResponse>();    // Id, Name, ...
        csvWriter.NextRecord();                     // \n [new_line]

        // Load the person data, then write it as CSV
        var persons = await _db.Persons
            .Include("Country")
            .Select(p => p.ToPersonResponse()).ToListAsync();
        await csvWriter.WriteRecordsAsync(persons); // 1,Ann, ...

        memoryStream.Position = 0;                  // reset the memory stream position
        return memoryStream;
    }
}
