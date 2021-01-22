using AutoMapper;
using BooksAPI.DTOs.AuthorDTOs;
using BooksAPI.Models;

namespace BooksAPI.DTOs.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorReadDto>();
            CreateMap<Book, BookForReadAuthorDto>();

            CreateMap<AuthorCreateDto, Author>();
            CreateMap<BookForCreateAuthorDto, Book>();

            CreateMap<AuthorUpdateDto, Author>();
        }
    }
}
