using ServiceContracts.DTO;
using Microsoft.AspNetCore.Http;
using Entities;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesAdderService
    {
        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns>Returns the country object after adding it (including newly generated county id)</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
    }
}
