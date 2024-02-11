using System.Linq.Expressions;
using Entities;

namespace RepositoryContracts;

/// <summary>
/// Represents data access logic for managin Person entity
/// </summary>
public interface IPersonRepository
{
    /// <summary>
    /// Add a person object to the data store
    /// </summary>
    /// <param name="person">Person object to add</param>
    /// <returns>Return the person object after adding it to the table</returns>
    Task<Person> AddPerson(Person person);

    /// <summary>
    /// Returns all persons in the data store
    /// </summary>
    /// <returns>List of person object from table</returns>
    Task<List<Person>> GetAllPersons();

    /// <summary>
    /// Returns a person object based on the given id
    /// </summary>
    /// <param name="id">Id to search</param>
    /// <returns>Person object or null</returns>
    Task<Person?> GetPersonById(Guid id);

    /// <summary>
    /// Returns all person object based on the given expression
    /// </summary>
    /// <param name="predicate">LINQ expression to check</param>
    /// <returns>All matching person with given condition</returns>
    Task<List<Person>> GetFilteredPerson(Expression<Func<Person, bool>> predicate);

    /// <summary>
    /// Delete a person object based on the given id
    /// </summary>
    /// <param name="id">Id to search</param>
    /// <returns>Returns true, if deletion success, otherwise false</returns>
    Task<bool> DeletePersonById(Guid id);

    /// <summary>
    /// Update a person object (person name and other details) based on the given id
    /// </summary>
    /// <param name="person">Person object to update</param>
    /// <returns>Returns the updated person object</returns>
    Task<Person?> UpdatePerson(Person person);
}
