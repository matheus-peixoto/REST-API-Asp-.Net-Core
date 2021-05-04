using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BooksAPI.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public List<AuthorBook> AuthorsBooks { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Sinopse { get; set; }
        public DateTime RegisterDate { get; set; }

        public Book() { }

        public Book(int id, string title, DateTime releaseDate, string sinopse)
        {
            Id = id;
            Title = title;
            ReleaseDate = releaseDate;
            Sinopse = sinopse;
        }
    }
}
