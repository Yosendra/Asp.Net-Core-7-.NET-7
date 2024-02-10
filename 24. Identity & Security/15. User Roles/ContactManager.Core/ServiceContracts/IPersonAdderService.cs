using ServiceContracts.DTO;

namespace ServiceContracts;

/// <summary>
/// Represents business logic for manipulating Person entity
/// </summary>
public interface IPersonAdderService
{
    /// <summary>
    /// Adds a new person into the list of person
    /// </summary>
    /// <param name="requestModel">Person to add</param>
    /// <returns>Returns the same person detail along with newly generated Id</returns>
    Task<PersonResponse> AddPerson(PersonAddRequest? requestModel);
}
