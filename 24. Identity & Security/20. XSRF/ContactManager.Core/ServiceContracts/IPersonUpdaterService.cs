using ServiceContracts.DTO;

namespace ServiceContracts;

/// <summary>
/// Represents business logic for manipulating Person entity
/// </summary>
public interface IPersonUpdaterService
{
    /// <summary>
    /// Update the specified person details based on the given id
    /// </summary>
    /// <param name="requestModel">Person details to update including person id</param>
    /// <returns>Return the person response object after updation</returns>
    Task<PersonResponse> UpdatePerson(PersonUpdateRequest? requestModel);
}
