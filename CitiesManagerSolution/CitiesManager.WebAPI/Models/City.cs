using System.ComponentModel.DataAnnotations;

namespace CitiesManager.WebAPI.Models
{
    public class City
    {
        [Key]
        public Guid CityID { get; set; }
        [Required(ErrorMessage = "City Name can not be blank")]
        public string? CityName { get; set; }
    }
}
