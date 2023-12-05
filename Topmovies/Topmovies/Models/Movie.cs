using System.ComponentModel.DataAnnotations;

namespace Topmovies.Models
{
    public class Movie
    {
        
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string PicturePath { get; set; }
    }
}
