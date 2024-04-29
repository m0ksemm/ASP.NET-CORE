using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class CountriesDeleterService : ICountriesDeleterService
    {
        //private field
        private readonly ICountriesRepository _countriesRepository;

        //constructor
        public CountriesDeleterService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public async Task<bool> DeleteCountry(Guid? countryID)
        {
            if (countryID == null)
            {
                throw new ArgumentNullException(nameof(countryID));
            }

            Country? country = await _countriesRepository.GetCountryByCountryId(countryID.Value);
            if (country == null)
                return false;

            await _countriesRepository.DeleteCountry(countryID.Value);

            return true;
        }
    }
}
