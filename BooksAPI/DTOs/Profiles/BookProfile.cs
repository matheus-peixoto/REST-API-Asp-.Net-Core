using AutoMapper;
using BooksAPI.DTOs.BookDTOs;
using BooksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.DTOs.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookReadDto>();
            CreateMap<Author, AuthorForReadBookDto>();
        }
    }
}
