using ServiceContracts.DTO;
using ServiceContracts.Enums;

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
    List<PersonResponse> GetAllPersons();

    /// <summary>
    /// Returns the person object based on the given id
    /// </summary>
    /// <param name="id">Person id to search</param>
    /// <returns>Returns matching person object</returns>
    PersonResponse? GetPersonById(Guid? id);

    /// <summary>
    /// Return all person objects that match with the given search field and keyword
    /// </summary>
    /// <param name="searchBy">Search field to search</param>
    /// <param name="keyword">Search keyword to search</param>
    /// <returns>Return all matching persons based on the given search field and keyword</returns>
    List<PersonResponse> GetFilteredPersons(string searchBy, string? keyword);

    /// <summary>
    /// Return sorted list of persons
    /// </summary>
    /// <param name="allPersons">Represent list of persons to be sorted</param>
    /// <param name="sortBy">Name of the property (key), based on which the persons should be sorted</param>
    /// <param name="sortOrder">Ascending (ASC) or Descending (DESC)</param>
    /// <returns>Return sorted persons as PersonResponse list</returns>
    List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderEnum sortOrder);

    /// <summary>
    /// Update the specified person details based on the given id
    /// </summary>
    /// <param name="requestModel">Person details to update including person id</param>
    /// <returns>Return the person response object after updation</returns>
    PersonResponse UpdatePerson(PersonUpdateRequest? requestModel);

    /// <summary>
    /// Delete a person based on the given id
    /// </summary>
    /// <param name="id">Id of the person</param>
    /// <returns>Return true if the deletion is success, otherwise false</returns>
    bool DeletePerson(Guid? id);
}
