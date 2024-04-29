using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsUpdaterService
    {
        /// <summary>
        /// Updates the specified person details based on the given person ID
        /// </summary>
        /// <param name="personUpdateRequest">Person details to update, including person id</param>
        /// <returns>Returns the person response object after update</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
    }
}
