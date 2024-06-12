using ServiceContracts.DTO;
using Microsoft.AspNetCore.Http;
using Entities;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesUpdaterService
    {
        /// <summary>
        /// Updates the country in database
        /// </summary>
        /// <param name="countryUpdateRequest">Reieves the new data about country</param>
        /// <returns>Returns updated country data</returns>
        Task<CountryResponse> UpdateCountry(CountryUpdateRequest? countryUpdateRequest);
    }
}
