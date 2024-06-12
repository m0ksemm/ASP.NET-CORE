using ServiceContracts.DTO;
using Microsoft.AspNetCore.Http;
using Entities;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesDeleterService
    {
        /// <summary>
        /// Deletes country row from database
        /// </summary>
        /// <param name="countryID">Receives the countryID of country that has to be deleted</param>
        /// <returns>Returns true when the deletion is successfull, otherwise returns false</returns>
        Task<bool> DeleteCountry(Guid? countryID);
       
    }
}
