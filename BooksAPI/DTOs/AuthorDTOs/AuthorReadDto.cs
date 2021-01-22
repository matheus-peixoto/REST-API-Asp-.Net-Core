using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.DTOs.AuthorDTOs
{
    public class AuthorReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public BookForReadAuthorDto[] Books { get; set; }
        public DateTime BirthDate { get; set; }
        public string ShortBio { get; set; }
    }
}
