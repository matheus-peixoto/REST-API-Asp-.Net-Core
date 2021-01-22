using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BooksAPI.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AuthorBook> AuthorsBooks { get; set; }
        public DateTime BirthDate { get; set; }
        public string ShortBio { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
