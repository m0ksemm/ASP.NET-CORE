using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class CountryUpdateRequest
    {
        [Required(ErrorMessage = "Country ID can't be blank")]
        public Guid CountryID { get; set; }
        [Required(ErrorMessage = "CountryName can't be blank")]
        public string? CountryName { get; set; }

        /// <summary>
        /// Converts the current object of PersonAddRequest inti a new object of Person type
        /// </summary>
        /// <returns>Returns Person object</returns>
        public Country ToCountry()
        {
            return new Country()
            {
                CountryID = CountryID,
                CountryName = CountryName,
            };
        }
    }
}
