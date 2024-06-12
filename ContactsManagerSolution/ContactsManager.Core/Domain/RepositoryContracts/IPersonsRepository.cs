using Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Person entity
    /// </summary>
    public interface IPersonsRepository
    {
        /// <summary>
        /// Adds a person object to the data store
        /// </summary>
        /// <param name="person">Person object ot add</param>
        /// <returns>Returns the person object after adding it to the table</returns>
        Task<Person> AddPerson(Person person);

        /// <summary>
        /// Returns all persons in the data store
        /// </summary>
        /// <returns>List of person objects from table</returns>
        Task<List<Person>> GetAllPersons();

        /// <summary>
        /// Returns a person object based on the given person id
        /// </summary>
        /// <param name="personID">PersonID</param>
        /// <returns>A person object or null</returns>
        Task<Person?> GetPersonByPersonID(Guid personID);

        /// <summary>
        /// Returns all person objects based on the given expression
        /// </summary>
        /// <param name="predicate">LINQ expressin to check</param>
        /// <returns>All matching persons with given condition</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

        /// <summary>
        /// Deletes a person object based on the person id
        /// </summary>
        /// <param name="personID">Person ID (guid) to search</param>
        /// <returns>Returns true, if the deletion is successful; otherwise returns false</returns>
        Task<bool> DeletePerson(Guid personID);

        /// <summary>
        /// Updates a person object (person name and other details) based on the given person id
        /// </summary>
        /// <param name="person">Person object to update</param>
        /// <returns>Returns the updated person object</returns>
        Task<Person> UpdatePerson(Person person);
    }
}
