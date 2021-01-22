using AutoMapper;
using BooksAPI.DTOs.AuthorDTOs;
using BooksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.DTOs.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorReadDto>();
            CreateMap<Book, BookForReadAuthorDto>();
        }
    }
}
