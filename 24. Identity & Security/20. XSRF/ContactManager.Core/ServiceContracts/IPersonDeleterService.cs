namespace ServiceContracts;

/// <summary>
/// Represents business logic for manipulating Person entity
/// </summary>
public interface IPersonDeleterService
{
    /// <summary>
    /// Delete a person based on the given id
    /// </summary>
    /// <param name="id">Id of the person</param>
    /// <returns>Return true if the deletion is success, otherwise false</returns>
    Task<bool> DeletePerson(Guid? id);
}
