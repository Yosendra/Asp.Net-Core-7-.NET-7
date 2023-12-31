using ServiceContracts.DTO;

namespace ServiceContracts;

/// <summary>
/// Represents business logic for manipulating Person entity
/// </summary>
public interface IPersonService
{
    /// <summary>
    /// Adds a new person into the list of person
    /// </summary>
    /// <param name="requestModel">Person to add</param>
    /// <returns>Returns the same person detail along with newly generated Id</returns>
    PersonResponse AddPerson(PersonAddRequest? requestModel);

    /// <summary>
    /// Return all person
    /// </summary>
    /// <returns>Returns a list of object of PersonResponse type</returns>
    List<PersonResponse> GetAllPerson();

    /// <summary>
    /// Returns the person object based on the given id
    /// </summary>
    /// <param name="id">Person id to search</param>
    /// <returns>Returns matching person object</returns>
    PersonResponse? GetPersonById(Guid? id);
}
