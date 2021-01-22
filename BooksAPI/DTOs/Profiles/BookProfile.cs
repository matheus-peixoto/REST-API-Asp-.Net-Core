using AutoMapper;
using BooksAPI.DTOs.BookDTOs;
using BooksAPI.Models;

namespace BooksAPI.DTOs.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookReadDto>();
            CreateMap<Author, AuthorForReadBookDto>();

            CreateMap<BookCreateDto, Book>();
            CreateMap<AuthorForCreateBookDto, Author>();
        }
    }
}
