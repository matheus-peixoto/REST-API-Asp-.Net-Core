using AutoMapper;
using BooksAPI.DTOs.BookDTOs;
using BooksAPI.Models;
using BooksAPI.Repositorys.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.Controllers
{
    [ApiController]
    [Route("books")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<BookReadDto>>> Get()
        {
            List<Book> booksFromDb = await _bookRepository.FindAllAsync();

            List<BookReadDto> bookReadDtos = new List<BookReadDto>();
            foreach (Book book in booksFromDb)
            {
                BookReadDto bookReadDto = _mapper.Map<BookReadDto>(book);
                Author[] books = book.AuthorsBooks.Select(ab => ab.Author).ToArray();
                bookReadDto.Authors = _mapper.Map<AuthorForReadBookDto[]>(books);
                bookReadDtos.Add(bookReadDto);
            }

            return Ok(bookReadDtos);
        }
    }
}
