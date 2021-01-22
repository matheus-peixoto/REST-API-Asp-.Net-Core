using System.ComponentModel.DataAnnotations;

namespace BooksAPI.DTOs.AuthorDTOs
{
    public class AuthorUpdateDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "The name should have at least 3 characters")]
        [MaxLength(60, ErrorMessage = "The name should have at maximum 60 characters")]
        public string Name { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The name should have at least 3 characters")]
        [MaxLength(500, ErrorMessage = "The name should have at maximum 60 characters")]
        public string ShortBio { get; set; }
    }
}
