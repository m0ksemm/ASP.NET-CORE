using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsGetterService
    {
        /// <summary>
        /// Returns all persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse type</returns>
        Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Returns the person object based on the given person id
        /// </summary>
        /// <param name="personUD">Person id to search</param>
        /// <returns>Matching person object</returns>
        Task<PersonResponse?> GetPersonByPersonID(Guid? personID);

        /// <summary>
        /// Returns all person objects that matches with the given search field and search string
        /// </summary>
        /// <param name="searchBy">Search field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>Returns all matching persons based on the given search fields and search string</returns>
        Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string? searchString);

        /// <summary>
        /// Returns persons as CSV
        /// </summary>
        /// <returns>Returns the memory stream with SCV</returns>
        Task<MemoryStream> GetPersonsCSV();

        /// <summary>
        /// Return persons as Excel
        /// </summary>
        /// <returns>Returns the memory stream with Excel data of persons</returns>
        Task<MemoryStream> GetPersonsExcel();
    }
}
