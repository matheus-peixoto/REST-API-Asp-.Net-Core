using System;
using System.ComponentModel.DataAnnotations;

namespace BooksAPI.DTOs.BookDTOs
{
    public class BookUpdateDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "The title needs to have at least {0} character")]
        [MaxLength(100, ErrorMessage = "The title can have {0} characters at maximum")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "You need to pass the date")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The title needs to have at least {0} characters")]
        [MaxLength(250, ErrorMessage = "The title can have {0} characters at maximum")]
        public string Sinopse { get; set; }
    }
}
