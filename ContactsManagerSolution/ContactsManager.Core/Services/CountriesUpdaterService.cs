using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class CountriesUpdaterService : ICountriesUpdaterService
    {
        //private field
        private readonly ICountriesRepository _countriesRepository;

        //constructor
        public CountriesUpdaterService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public async Task<CountryResponse> UpdateCountry(CountryUpdateRequest? countryUpdateRequest)
        {
            if (countryUpdateRequest == null)
                throw new ArgumentNullException(nameof(countryUpdateRequest));

            //validation
            ValidationHelper.ModelValidation(countryUpdateRequest);

            //get matching person object to update
            Country? matchingCountry = await _countriesRepository.GetCountryByCountryId(countryUpdateRequest.CountryID);
            if (matchingCountry == null)
            {
                throw new ArgumentException("Given person id doesn't exist");
            }

            //update all details
            matchingCountry.CountryName = countryUpdateRequest.CountryName;

            await _countriesRepository.UpdateCountry(matchingCountry); //UPDATE

            return matchingCountry.ToCountryResponse();
        }
    }
}
