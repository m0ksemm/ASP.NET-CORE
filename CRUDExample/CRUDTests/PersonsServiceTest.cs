using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using Entities;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using AutoFixture;
using FluentAssertions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        ///private fields
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        //constructor
        public PersonsServiceTest(ITestOutputHelper testOutputHelper) 
        {
            _fixture = new Fixture();

            var countriesInitialData = new List<Country>() { };
            var personsInitialData = new List<Person>() { };

            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
              new DbContextOptionsBuilder<ApplicationDbContext>().Options
             );

            ApplicationDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitialData);

            _countriesService = new CountriesService(dbContext);
           
            _personsService = new PersonsService(dbContext, _countriesService);
            
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson

        //When we supply null value as PersonAddRequest, it should throw ArgumentNullException
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Act
            Func<Task> action = (async () => 
            {
                 await _personsService.AddPerson(personAddRequest);
            });
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply null value as PersonName, it should throw ArgumentException
        [Fact]
        public async Task AddPerson_PersonNameNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = _fixture
                .Build<PersonAddRequest>()
                .With(temp => temp.PersonName, null as string)
                .Create();

            //Assert
            Func<Task> action = (async () =>
            {
                //Act
                await _personsService.AddPerson(personAddRequest);
            });
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When we supply proper person details, it should insert the person into the persons list; and it should return ArgumentNullException object of PersonResponse, which includes with the newly generated person id
        [Fact]
        public async Task AddPerson_ProperPersonDetails()
        {
            //Arrange
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone@example.com")
                .Create();
            
            //Act
            PersonResponse person_response_from_add = await _personsService.AddPerson(personAddRequest);
            List<PersonResponse> person_list = await _personsService.GetAllPersons();

            //Assert
            //Assert.True(person_response_from_add.PersonID != Guid.Empty);
            person_response_from_add.PersonID.Should().NotBe(Guid.Empty);

            //Assert.Contains(person_response_from_add, person_list);
            person_list.Should().Contain(person_response_from_add);
        }

        #endregion

        #region GetPersonPersonID

        //If we supply null as PersonID, it should return null as PersonResponse
        [Fact]
        public async Task GetPersonByPersonID_NullPersonID()
        {
            //Arrange
            Guid? personID = null;

            //Act
            PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonID(personID);

            //Assert
            //Assert.Null(person_response_from_get);
            person_response_from_get.Should().BeNull();
        }

        //If we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public async Task GetPersonByPersonID_WithPersonID()
        {
            //Arrange
            CountryAddRequest countryRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse country_response = await _countriesService.AddCountry(countryRequest);

            //PersonAddRequest personAddRequest
            PersonAddRequest person_request = _fixture
                .Build<PersonAddRequest>()
                .With(temp => temp.Email, "email@sample.com")
                .Create();
            
            PersonResponse person_response_from_add = await _personsService.AddPerson(person_request);

            PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonID(person_response_from_add.PersonID);

            //Assert
            //Assert.Equal(person_response_from_add, person_response_from_get);
            person_response_from_get.Should().Be(person_response_from_add);
        }

        #endregion

        #region GetAllPersons

        //The GetAllPerson() should return an empty list by default 
        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> persons_from_get = await _personsService.GetAllPersons();

            //Assert
            //Assert.Empty(persons_from_get);
            persons_from_get.Should().BeEmpty();
        }

        //First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added 
        [Fact]
        public async Task GetAllPersons_AddFewPersons() 
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_1@example.com").Create();
            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_2@example.com").Create();
            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_3@example.com").Create();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> person_list_from_get = await _personsService.GetAllPersons();

            //print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_response_from_get in person_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //Assert
            person_list_from_get.Should().BeEquivalentTo(person_response_list_from_add);
        }
        #endregion

        #region GetFilteredPersons 
        //If the search text os empty and search by is "PersonName", it should return all persons 
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_1@example.com").Create();
            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_2@example.com").Create();
            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "someone_3@example.com").Create();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> person_list_from_search = await _personsService.GetFilteredPerson(nameof(PersonResponse.PersonName), "");

            //Assert
            person_list_from_search.Should().BeEquivalentTo(person_response_list_from_add);
        }

        //First we will add few persons; and then we will search based on person name with some search string. It should return the matching persons
        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone_1@example.com")
                .With(temp => temp.PersonName, "Rahman")
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .Create();
            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone_2@example.com")
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .With(temp => temp.PersonName, "Mary")
                .Create();
            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone_3@example.com")
                .With(temp => temp.PersonName, "James")
                .With(temp => temp.CountryID, country_response_2.CountryID)
                .Create();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> person_list_from_search = await _personsService.GetFilteredPerson(nameof(PersonResponse.PersonName), "ma");

            //print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_response_from_get in person_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            person_list_from_search
                .Should()
                .OnlyContain(temp => temp.PersonName
                .Contains("ma", StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region GetSortedPersons

        //When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
        [Fact]
        public async Task GetSortedPersons()
        {
            //Arrange
            CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone_1@example.com")
                .With(temp => temp.PersonName, "Smith")
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .Create();
            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone_2@example.com")
                .With(temp => temp.CountryID, country_response_1.CountryID)
                .With(temp => temp.PersonName, "Mary")
                .Create();
            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone_3@example.com")
                .With(temp => temp.PersonName, "Rahman")
                .With(temp => temp.CountryID, country_response_2.CountryID)
                .Create();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            List<PersonResponse> allPersons = await _personsService.GetAllPersons();
            //Act
            List<PersonResponse> person_list_from_sort = await _personsService.GetSortedPersons(allPersons, nameof(PersonResponse.PersonName), SortOrderOptions.DESC);

            //print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_response_from_get in person_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }
            person_response_list_from_add = person_response_list_from_add.OrderByDescending(temp => temp.PersonName).ToList();

            //Assert
            //person_list_from_sort.Should().BeEquivalentTo(person_response_list_from_add);

            person_list_from_sort.Should().BeInDescendingOrder(temp => temp.PersonName);
        }
        #endregion

        #region UpdatePerson

        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = null;

            Func<Task> action = async () =>
            {
                //Act
                await _personsService.UpdatePerson(person_update_request);
            };
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply invalid person id, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            //Arrange
            PersonUpdateRequest person_update_request = _fixture.Build<PersonUpdateRequest>().Create();

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _personsService.UpdatePerson(person_update_request);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When PersonName is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonNameIsNull()
        {
            //Arrange
            CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response = await _countriesService.AddCountry(country_request);

            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone_1@example.com")
                .With(temp => temp.PersonName, "Smith")
                .With(temp => temp.CountryID, country_response.CountryID)
                .Create();

            PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);
            PersonUpdateRequest personUpdateRequest = person_response_from_add.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

           Func<Task> action = (async () =>
            {
                //Act
                await _personsService.UpdatePerson(personUpdateRequest);
            });
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }
        // +Email Can Not Be Null
        // ....

        //First, add a new person and try to update the person name and email
        [Fact]
        public async Task UpdatePerson_PersonFullDetailsUpdate()
        {
            //Arrange
            CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response = await _countriesService.AddCountry(country_request);

            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone_1@example.com")
                .With(temp => temp.PersonName, "Smith")
                .With(temp => temp.CountryID, country_response.CountryID)
                .Create();

            PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = "William";
            person_update_request.Email = "william@example.com";

            //Act
            PersonResponse person_response_from_update = await _personsService.UpdatePerson(person_update_request);

            PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonID(person_response_from_update.PersonID);

            //Assert
            Assert.Equal(person_response_from_get, person_response_from_update);

            person_response_from_update.Should().Be(person_response_from_get);
        }
        #endregion

        #region DeletePerson

        //If tou supply an valid PersonID, it should return true
        [Fact]
        public async Task DeletePerson_ValidPersonID()
        {
            //Arrange
            CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response = await _countriesService.AddCountry(country_request);

            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone_1@example.com")
                .With(temp => temp.PersonName, "Smith")
                .With(temp => temp.CountryID, country_response.CountryID)
                .Create();

            PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);

            //Act
            bool isDeleted = await _personsService.DeletePerson(person_response_from_add.PersonID);

            //Assert
            isDeleted.Should().BeTrue();
        }

        //If tou supply an invalid PersonID, it should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonID()
        {
            //Act
            bool isDeleted = await _personsService.DeletePerson(Guid.NewGuid());

            //Assert
            isDeleted.Should().BeFalse();
        }
        #endregion
    }
}
