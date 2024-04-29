using CRUDExample.Filters.ActionFilters;
using CRUDExample.Filters.AuthorizationFilter;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        private readonly ICountriesAdderService _countriesAdderService;
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ICountriesUploaderService _countriesUploaderService;
        private readonly ICountriesUpdaterService _countriesUpdaterService;
        private readonly ICountriesDeleterService _countriesDeleterService;


        private readonly IPersonsGetterService _personsGetterService;

        public CountriesController(ICountriesAdderService countriesAdderService, ICountriesGetterService countriesGetterService, ICountriesUploaderService countriesUploaderService, ICountriesUpdaterService countriesUpdaterService, ICountriesDeleterService countriesDeleterService, IPersonsGetterService personsService)
        {
            _countriesAdderService = countriesAdderService;
            _countriesGetterService = countriesGetterService;
            _countriesUploaderService = countriesUploaderService;
            _countriesUpdaterService = countriesUpdaterService;
            _countriesDeleterService = countriesDeleterService;

            _personsGetterService = personsService;
        }

        [Route("UploadFromExcel")]
        public IActionResult UploadFromExcel()
        {
            return View();
        }

        [HttpPost]
        [Route("UploadFromExcel")]
        public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please select an xlsx file";
                return View();
            }
            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "Unsupported file. 'xlsx' file is expected";
                return View();
            }

            int countriesCountInserted = await _countriesUploaderService.UploadCountriesFromExcelFile(excelFile);
            ViewBag.Message = $"{countriesCountInserted} Countries Uploaded";
            return View();
        }

        [Route("[action]")]
        [Route("Countries")]
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {

            //Search
            ViewBag.SearchFields = new Dictionary<string, string>()
              {
                { nameof(CountryResponse.CountryID), "Country Name" },
              };

            List<CountryResponse> countries = await _countriesGetterService.GetAllCountries();
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;


            return View(countries); //Views/Persons/Index.cshtml
        }


        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        //Url: persons/create
        [Route("[action]")]
        public async Task<IActionResult> Create(CountryAddRequest countryAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = await _countriesGetterService.GetAllCountries();
                ViewBag.Countries = countries.Select(temp =>
                new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(countryAddRequest);
            }

            //call the service method
            CountryResponse countryResponse = await _countriesAdderService.AddCountry(countryAddRequest);

            //navigate to Index() action method (it makes another get request to "persons/index"
            return RedirectToAction("Index", "Countries");
        }


        [HttpGet]
        [Route("[action]/{countryID}")] //Eg: /persons/edit/1
        //[TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Edit(Guid countryID)
        {
            CountryResponse? countryResponse = await _countriesGetterService.GetCountryByCountryID(countryID);
            if (countryResponse == null)
            {
                return RedirectToAction("Index");
            }

            CountryUpdateRequest countryUpdateRequest = countryResponse.ToCountryUpdateRequest();

           

            return View(countryUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{countryID}")]
        //[TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        //[TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit(CountryUpdateRequest countryRequest)
        {
            List<CountryResponse> countries = await _countriesGetterService.GetAllCountries();

            if (countries.Where(country => country.CountryName == countryRequest.CountryName).Count() != 0)
            {
                ViewBag.Error = "This country already exists!";

                return View(countryRequest);
            }


            ViewBag.Countries = countries.Select(temp => new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

            CountryResponse? countryResponse = await _countriesGetterService.GetCountryByCountryID(countryRequest.CountryID);
            if (countryResponse == null)
            {
                return RedirectToAction("Index");
            }
            CountryResponse updatedCountry = await _countriesUpdaterService.UpdateCountry(countryRequest);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]/{countryID}")]
        public async Task<IActionResult> Delete(Guid? countryID)
        {
            CountryResponse? countryResponse = await _countriesGetterService.GetCountryByCountryID(countryID);
            if (countryResponse == null)
            {
                return RedirectToAction("Index");
            }

            return View(countryResponse);
        }

        [HttpPost]
        [Route("[action]/{countryID}")]
        public async Task<IActionResult> Delete(CountryUpdateRequest countryUpdateResult)
        {
            CountryResponse? countryResponse = await _countriesGetterService.GetCountryByCountryID(countryUpdateResult.CountryID);

            if (countryResponse == null)
            {
                RedirectToAction("Index");
            }

            List<PersonResponse> persons = await _personsGetterService.GetAllPersons();

            if (persons.Where(person => person.CountryID == countryUpdateResult.CountryID).Count() != 0)
            {
                ViewBag.Error = "There are workers from this country, you can not remove it!";

                return View(countryResponse);
            }

            await _countriesDeleterService.DeleteCountry(countryUpdateResult.CountryID);
            return RedirectToAction("Index");
        }

    }
}
