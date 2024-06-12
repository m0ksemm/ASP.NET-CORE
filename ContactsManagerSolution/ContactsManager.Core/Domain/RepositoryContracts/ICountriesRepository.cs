using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Country entity
    /// </summary>
    public interface ICountriesRepository
    {
        /// <summary>
        /// Adds a new country object to the data store
        /// </summary>
        /// <param name="country">Country object to add</param>
        /// <returns>Country object after adding it to the data store</returns>
        Task<Country> AddCountry(Country country);

        /// <summary>
        /// Returns all countries in the data store
        /// </summary>
        /// <returns>All countries from the table</returns>
        Task<List<Country>> GetAllCountries();

        /// <summary>
        /// Returns a country object based on the given country id; otherwise, it returns null
        /// </summary>
        /// <param name="countryID">CountryID to search</param>
        /// <returns>Matching country or null</returns>
        Task<Country?> GetCountryByCountryId(Guid countryID);

        /// <summary>
        /// Returns a country object based on the given country name; otherwise, it returns null
        /// </summary>
        /// <param name="CountryName"></param>
        /// <returns>Matching country or null</returns>
        Task<Country?> GetCountryByCountryName(string CountryName);

        /// <summary>
        /// Deletes a country object based on the country id
        /// </summary>
        /// <param name="countryID">Country ID (guid) to search</param>
        /// <returns>Returns true, if the deletion is successful; otherwise returns false</returns>
        Task<bool> DeleteCountry(Guid countryID);

        /// <summary>
        /// Updates a country object (country name and other details) based on the given country id
        /// </summary>
        /// <param name="country">Country object to update</param>
        /// <returns>Returns the updated country object</returns>
        Task<Country> UpdateCountry(Country country);
    }
}
