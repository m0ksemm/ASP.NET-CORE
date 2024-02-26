using ServiceContracts;
using System;
using Entities;
using ServiceContracts.DTO;
using System.ComponentModel.DataAnnotations;
using Services.Helpers;
using ServiceContracts.Enums;
using System.Linq.Expressions;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        //private field
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        //constructor
        public PersonsService(bool initialize = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();

            if (initialize)
            {

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("FCAE41CF-FE9E-4E43-B1D3-3CBE3C7A4604"),
                    PersonName = "Marget",
                    Email = "mwessel0@fc2.com",
                    DateOfBirth = DateTime.Parse("2000-04-09"),
                    Gender = "Female",
                    Address = "053 Fordem Lane",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("779889D2-5863-40DF-A8D6-490DACC6EE17")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("369A1D13-617D-4829-9590-3120DB80A707"),
                    PersonName = "Tate",
                    Email = "tspurrior1@engadget.com",
                    DateOfBirth = DateTime.Parse("1995-08-15"),
                    Gender = "Female",
                    Address = "87533 Holy Cross Alley",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("23F89678-3309-4927-B69F-D438E963834E")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("FA0DDFA2-7676-4B3F-BE0C-C80D96E1CBC9"),
                    PersonName = "Hammad",
                    Email = "hrives2@flickr.com",
                    DateOfBirth = DateTime.Parse("2000-09-28"),
                    Gender = "Female",
                    Address = "Male,794 Derek Trail",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("23F89678-3309-4927-B69F-D438E963834E")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("A25D911D-5A2A-4A0C-B06A-7A0B1C15B421"),
                    PersonName = "Irwin",
                    Email = "ikanzler3@addtoany.com",
                    DateOfBirth = DateTime.Parse("1998-02-21"),
                    Gender = "Male",
                    Address = "242 Kropf Center",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("19EAB747-1C77-442E-99F4-E859ECBA9694")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("D3F239B8-7D22-497C-B6C4-32FA37D1634A"),
                    PersonName = "Philis",
                    Email = "pdibner4@feedburner.com",
                    DateOfBirth = DateTime.Parse("2000-09-07"),
                    Gender = "Male",
                    Address = "667 Forest Run Parkway",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("19EAB747-1C77-442E-99F4-E859ECBA9694")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("EF889FD6-D3A5-4541-B898-5EF725EFF48E"),
                    PersonName = "Henrik",
                    Email = "haguirre5@odnoklassniki.ua",
                    DateOfBirth = DateTime.Parse("2000-04-11"),
                    Gender = "Male",
                    Address = "637 Granby Crossing",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("6549520F-49E1-462A-930B-907B103EF570")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("AEB4771F-D0D6-4FE9-BAB4-6DB80EA18BE1"),
                    PersonName = "Hilda",
                    Email = "hkilner6@howstuffworks.com",
                    DateOfBirth = DateTime.Parse("1994-09-02"),
                    Gender = "Female",
                    Address = "361 Golf View Parkway",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("D7A7B8AE-C955-4888-BC4B-9C08E238563E")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("73E397F7-1E20-4ADA-8682-F53CB809657D"),
                    PersonName = "Neila",
                    Email = "ntuminini7@nationalgeographic.com",
                    DateOfBirth = DateTime.Parse("1996-03-07"),
                    Gender = "Female",
                    Address = "Stone Corner Junction",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("19EAB747-1C77-442E-99F4-E859ECBA9694")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("B372359E-BE9E-4D6F-88BD-73BF7A2AD545"),
                    PersonName = "Prent",
                    Email = "pkillford8@zdnet.com",
                    DateOfBirth = DateTime.Parse("1996-11-23"),
                    Gender = "Male",
                    Address = "2675 Walton Crossing",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("23F89678-3309-4927-B69F-D438E963834E")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("DAEFF1BC-93A6-467F-9345-041E7253D458"),
                    PersonName = "Melisa",
                    Email = "mpenhalewick9@desdev.cn",
                    DateOfBirth = DateTime.Parse("1990-10-14"),
                    Gender = "Female",
                    Address = "69 Eagle Crest Terrace",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("779889D2-5863-40DF-A8D6-490DACC6EE17")
                });
            }
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;
            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            //check of PersonAddRequest is not null
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Model validation
            ValidationHelper.ModelValidation(personAddRequest);

            //convert personAddRequest into Person type
            Person person = personAddRequest.ToPerson();

            //Generate PersonID
            person.PersonID = Guid.NewGuid();

            //add person object to persons list
            _persons.Add(person);

            //convert the Persons object into PersonResponse type
            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(temp => temp.ToPersonResponse()).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if (personID == null)
            {
                return null;
            }

            Person? person = _persons.FirstOrDefault(temp => temp.PersonID == personID);
            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();
        }

        public List<PersonResponse> GetFilteredPerson(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return matchingPersons;
            }

            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    matchingPersons = allPersons.Where(temp => 
                    (!string.IsNullOrEmpty(temp.PersonName) ? 
                    temp.PersonName.Contains(searchString, 
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Email):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Email) ?
                    temp.Email.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.DateOfBirth):
                    matchingPersons = allPersons.Where(temp =>
                    (temp.DateOfBirth != null) ?
                    temp.DateOfBirth.Value.ToString("dd MMM yyyy").Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Gender):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Gender) ?
                    temp.Gender.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.CountryID):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Country) ?
                    temp.Country.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Address):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Address) ?
                    temp.Address.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default: matchingPersons = allPersons; 
                    break;
            }
            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return allPersons;
            }

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),
                (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

                _ => allPersons
            };

            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            //Validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            //Get matching person object to update
            Person? matchingPerson = _persons.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID);
            if (matchingPerson == null)
            {
                throw new ArgumentException("Given person ID doesn't exist");
            }

            //Update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            return matchingPerson.ToPersonResponse();
        }

        public bool DeletePerson(Guid? personID)
        {
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID));
            }

            Person? person = _persons.FirstOrDefault(temp => temp.PersonID == personID);
            if (person == null)
            {
                return false;
            }

            _persons.RemoveAll(temp => temp.PersonID == personID);
            return true;
        }
    }
}
