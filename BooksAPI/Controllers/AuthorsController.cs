using AutoMapper;
using BooksAPI.DTOs.AuthorDTOs;
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
    [Route("authors")]
    public class AuthorsController: ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepository authorRepository, IBookRepository bookRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Get()
        {
            List<Author> authorsFromDb = await _authorRepository.FindAllAsync();
            List<AuthorReadDto> authorReadDtos = new List<AuthorReadDto>();
            foreach (Author author in authorsFromDb)
            {
                AuthorReadDto authorReadDto = _mapper.Map<AuthorReadDto>(author);
                List<Book> books = author.AuthorsBooks.Select(ab => ab.Book).ToList();
                authorReadDto.Books = _mapper.Map<BookForReadAuthorDto[]>(books);
                authorReadDtos.Add(authorReadDto);
            }

            return Ok(authorReadDtos);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetAuthorById")]
        public async Task<ActionResult<AuthorReadDto>> GetById(int id)
        {
            Author authorFromDb = await _authorRepository.FindByIdAsync(id);
            if (authorFromDb == null) return NotFound();

            AuthorReadDto authorReadDto = _mapper.Map<AuthorReadDto>(authorFromDb);
            Book[] books = authorFromDb.AuthorsBooks.Select(ab => ab.Book).ToArray();
            authorReadDto.Books = _mapper.Map<BookForReadAuthorDto[]>(books);
            return Ok(authorReadDto);
        }
    }
}
