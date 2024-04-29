using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsSorterService
    {
        /// <summary>
        /// Returns sorted list of persons
        /// </summary>
        /// <param name="allPersons">Represents list of persons to sort</param>
        /// <param name="sortBy">Name of the property(key), based on which the persons shounld be sorted</param>
        /// <param name="sortOrder">Returns sorted persons as PersonResponse list</param>
        /// <returns></returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);
        
    }
}
