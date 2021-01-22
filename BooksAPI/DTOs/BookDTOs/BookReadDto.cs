using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.DTOs.BookDTOs
{
    public class BookReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public AuthorForReadBookDto[] Authors { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Sinopse { get; set; }
    }
}
