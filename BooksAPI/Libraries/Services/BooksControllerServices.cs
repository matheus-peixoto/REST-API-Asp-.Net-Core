using AutoMapper;
using BooksAPI.DTOs.BookDTOs;
using BooksAPI.Models;
using BooksAPI.Repositorys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.Libraries.Services
{
    public class BooksControllerServices
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public BooksControllerServices(IMapper mapper, IAuthorRepository authorRepository)
        {
            _mapper = mapper;
            _authorRepository = authorRepository;
        }

        public BookReadDto GetBookReadDto(Book book)
        {
            BookReadDto bookDto = _mapper.Map<BookReadDto>(book);
            bookDto.Authors = book.AuthorsBooks
                .Select(ab => new AuthorForReadBookDto(ab.AuthorId, ab.Author.Name, ab.Author.BirthDate, ab.Author.ShortBio)).ToArray();
            return bookDto;
        }

        public List<BookReadDto> GetBooksReadDto(List<Book> books)
        {
            List<BookReadDto> bookReadDtos = new List<BookReadDto>();
            foreach (Book book in books)
            {
                bookReadDtos.Add(GetBookReadDto(book));
            }
            return bookReadDtos;
        }

        public async Task<Book> FilledBookOnCreateAsync(BookCreateDto bookDto)
        {
            Book book = _mapper.Map<Book>(bookDto);
            book.RegisterDate = DateTime.Now;
            List<Author> authors = await GetAuthorsOnCreateAsync(bookDto);
            book = FilledAuthorBooksOnCreateProperty(book, bookDto, authors);
            return book;
        }

        private Book FilledAuthorBooksOnCreateProperty(Book book, BookCreateDto bookDto, List<Author> authors)
        {
            List<AuthorBook> authorBooks = new List<AuthorBook>();
            foreach (Author author in authors)
            {
                if (AreAuthorsNewOnCreate(bookDto))
                    author.RegisterDate = DateTime.Now;
                authorBooks.Add(new AuthorBook() { Author = author, Book = book });
            }
            book.AuthorsBooks = authorBooks;
            return book;
        }

        private async Task<List<Author>> GetAuthorsOnCreateAsync(BookCreateDto bookDto)
        {
            List<Author> authors = new List<Author>();
            if (AreAuthorsNewOnCreate(bookDto))
                authors = _mapper.Map<List<Author>>(bookDto.Authors);
            else
                authors = await _authorRepository.FindAllWithFilterAsync(a => bookDto.AuthorsIds.Any(id => id == a.Id));

            return authors;
        }

        private bool AreAuthorsNewOnCreate(BookCreateDto bookDto) => !(bookDto.Authors is null) && bookDto.Authors.Length > 0;

    }
}
