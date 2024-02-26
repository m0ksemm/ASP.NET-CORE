using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //private field
        private readonly List<Country> _countries;

        //constructor
        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();
            if (initialize ) 
            {
                _countries.AddRange(new List<Country> {
                    new Country() { CountryID = Guid.Parse("779889D2-5863-40DF-A8D6-490DACC6EE17"), CountryName = "USA" },
                    new Country() { CountryID = Guid.Parse("23F89678-3309-4927-B69F-D438E963834E"), CountryName = "Canada" },
                    new Country() { CountryID = Guid.Parse("19EAB747-1C77-442E-99F4-E859ECBA9694"), CountryName = "UK" },
                    new Country() { CountryID = Guid.Parse("6549520F-49E1-462A-930B-907B103EF570"), CountryName = "India" },
                    new Country() { CountryID = Guid.Parse("D7A7B8AE-C955-4888-BC4B-9C08E238563E"), CountryName = "Australia" }
                });

            }
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {

            //Validation: countryAddRequest parameter can't be null
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //Validation: countryName can't be null
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            //Validation: CountryName can't be duplicate
            if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            //generate CountryID
            country.CountryID = Guid.NewGuid();

            //Add country object into _countries
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null)
            {
                return null;
            }
            Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryID == countryID);

            if (country_response_from_list == null)
                return null;

            return country_response_from_list.ToCountryResponse();
        }
    }
}
