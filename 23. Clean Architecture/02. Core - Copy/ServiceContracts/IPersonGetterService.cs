using ServiceContracts.DTO;

namespace ServiceContracts;

/// <summary>
/// Represents business logic for manipulating Person entity
/// </summary>
public interface IPersonGetterService
{
    /// <summary>
    /// Return all person
    /// </summary>
    /// <returns>Returns a list of object of PersonResponse type</returns>
    Task<List<PersonResponse>> GetAllPersons();

    /// <summary>
    /// Returns the person object based on the given id
    /// </summary>
    /// <param name="id">Person id to search</param>
    /// <returns>Returns matching person object</returns>
    Task<PersonResponse?> GetPersonById(Guid? id);

    /// <summary>
    /// Return all person objects that match with the given search field and keyword
    /// </summary>
    /// <param name="searchBy">Search field to search</param>
    /// <param name="keyword">Search keyword to search</param>
    /// <returns>Return all matching persons based on the given search field and keyword</returns>
    Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? keyword);

    /// <summary>
    /// Return persons as CSV
    /// </summary>
    /// <returns>Return the memory stream with CSV data</returns>
    Task<MemoryStream> GetPersonCSV();

    /// <summary>
    /// Return persons as Excel
    /// </summary>
    /// <returns>Return the memory stream with Excel data</returns>
    Task<MemoryStream> GetPersonExcel();
}
