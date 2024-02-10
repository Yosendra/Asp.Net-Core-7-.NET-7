using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts;

/// <summary>
/// Represents business logic for manipulating Person entity
/// </summary>
public interface IPersonSorterService
{
    /// <summary>
    /// Return sorted list of persons
    /// </summary>
    /// <param name="allPersons">Represent list of persons to be sorted</param>
    /// <param name="sortBy">Name of the property (key), based on which the persons should be sorted</param>
    /// <param name="sortOrder">Ascending (ASC) or Descending (DESC)</param>
    /// <returns>Return sorted persons as PersonResponse list</returns>
    Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderEnum sortOrder);
}
