using AutoMapper;
using BooksAPI.DTOs.AuthorDTOs;
using BooksAPI.Models;
using BooksAPI.Repositorys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.Libraries.Services
{
    public class AuthorsControllerServices
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public AuthorsControllerServices(IMapper mapper, IBookRepository bookRepository)
        {
            _mapper = mapper;
            _bookRepository = bookRepository;
        }

        public AuthorReadDto GetAuthorReadDto(Author author)
        {
            AuthorReadDto authorDto = _mapper.Map<AuthorReadDto>(author);
            authorDto.Books = author.AuthorsBooks.Select(ab => new BookForReadAuthorDto(ab.BookId, ab.Book.Title, ab.Book.ReleaseDate, ab.Book.Sinopse)).ToArray();
            return authorDto;
        }

        public List<AuthorReadDto> GetAuthorsReadDto(List<Author> authors)
        {
            List<AuthorReadDto> authorReadDtos = new List<AuthorReadDto>();
            foreach (Author author in authors)
            {
                AuthorReadDto authorReadDto = GetAuthorReadDto(author);
                authorReadDtos.Add(authorReadDto);
            }
            return authorReadDtos;
        }

        public async Task<Author> FilledAuthorOnCreateAsync(AuthorCreateDto authorDto)
        {
            Author author = _mapper.Map<Author>(authorDto);
            author.RegisterDate = DateTime.Now;
            List<Book> books = await GetBooksOnCreateAsync(authorDto);
            author = FilledAuthorBooksOnCreateProperty(author, authorDto, books);
            return author;
        }

        private Author FilledAuthorBooksOnCreateProperty(Author author, AuthorCreateDto authorDto, List<Book> books)
        {
            List<AuthorBook> authorBooks = new List<AuthorBook>();
            foreach (Book book in books)
            {
                if (AreBooksNewOnCreate(authorDto))
                    book.RegisterDate = DateTime.Now;
                authorBooks.Add(new AuthorBook() { Author = author, Book = book });
            }
            author.AuthorsBooks = authorBooks;
            return author;
        }

        private async Task<List<Book>> GetBooksOnCreateAsync(AuthorCreateDto authorDto)
        {
            List<Book> books = new List<Book>();
            if (AreBooksNewOnCreate(authorDto))
                books = _mapper.Map<List<Book>>(authorDto.Books);
            else
                books = await _bookRepository.FindAllWithFilterAsync(b => authorDto.BooksIds.Any(id => id == b.Id));


            return books;
        }

        private bool AreBooksNewOnCreate(AuthorCreateDto authorDto) => !(authorDto.Books is null) && authorDto.Books.Length > 0;
    }
}
