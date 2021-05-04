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

        public AuthorReadDto GetAuthorReadDtoOnCreate(Author author)
        {
            AuthorReadDto authorDto = _mapper.Map<AuthorReadDto>(author);
            List<Book> books = author.AuthorsBooks.Select(ab => new Book(ab.BookId, ab.Book.Title, ab.Book.ReleaseDate, ab.Book.Sinopse)).ToList();
            authorDto.Books = _mapper.Map<BookForReadAuthorDto[]>(books);
            return authorDto;
        }

        public async Task<Author> FilledOutAuthorOnCreateAsync(AuthorCreateDto authorDto)
        {
            Author author = _mapper.Map<Author>(authorDto);
            author.RegisterDate = DateTime.Now;
            List<Book> books = await GetBooksOnCreateAsync(authorDto);
            author = FilledOutAuthorBooksOnCreateProperty(author, authorDto, books);
            return author;
        }

        private Author FilledOutAuthorBooksOnCreateProperty(Author author, AuthorCreateDto authorDto, List<Book> books)
        {
            foreach (Book book in books)
            {
                if (AreBooksNewOnCreate(authorDto))
                    book.RegisterDate = DateTime.Now;
                author.AuthorsBooks.Add(new AuthorBook() { Author = author, Book = book });
            }
            return author;
        }

        private async Task<List<Book>> GetBooksOnCreateAsync(AuthorCreateDto authorDto)
        {
            List<Book> books = new List<Book>();
            if (!AreBooksNewOnCreate(authorDto))
                books = await _bookRepository.FindAllWithFilterAsync(b => authorDto.BooksIds.Any(id => id == b.Id));
            else
                books = _mapper.Map<List<Book>>(authorDto.Books);

            return books;
        }

        private bool AreBooksNewOnCreate(AuthorCreateDto authorDto) => !(authorDto.Books is null) && authorDto.Books.Length > 0;
    }
}
