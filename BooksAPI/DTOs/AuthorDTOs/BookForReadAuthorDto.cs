using System;

namespace BooksAPI.DTOs.AuthorDTOs
{
    public class BookForReadAuthorDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Sinopse { get; set; }

        public BookForReadAuthorDto() { }

        public BookForReadAuthorDto(int id, string title, DateTime releaseDate, string sinopse)
        {
            Id = id;
            Title = title;
            ReleaseDate = releaseDate;
            Sinopse = sinopse;
        }
    }
}
