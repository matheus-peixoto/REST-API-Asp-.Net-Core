﻿using AutoMapper;
using BooksAPI.DTOs.AuthorDTOs;
using BooksAPI.Models;
using BooksAPI.Repositorys.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Create([FromBody] AuthorCreateDto authorCreateDto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            Author author = _mapper.Map<Author>(authorCreateDto);
            author.RegisterDate = DateTime.Now;

            List<Book> books = new List<Book>();
            if (authorCreateDto.BooksIds != null && authorCreateDto.BooksIds.Length > 0)
                books = await _bookRepository.FindAllWithFilterAsync(b => authorCreateDto.BooksIds.Any(id => id == b.Id));
            else if (authorCreateDto.Books != null && authorCreateDto.Books.Length > 0)
                books = _mapper.Map<List<Book>>(authorCreateDto.Books);
            else
            {
                ModelState.AddModelError("Ids", "You need to pass the ids of the books that this author wrote or create the books that this author wrote");
                ModelState.AddModelError("Books", "You need to create the books that this author wrote or pass the ids of the books that this author wrote");
                return ValidationProblem(ModelState);
            }

            await _authorRepository.CreateAsync(author, books);

            AuthorReadDto authorReadDto = _mapper.Map<AuthorReadDto>(author);
            authorReadDto.Books = _mapper.Map<BookForReadAuthorDto[]>(books);

            return CreatedAtRoute("GetAuthorById", new { author.Id }, authorReadDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] AuthorUpdateDto authorUpdateDto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            Author author = await _authorRepository.FindByIdAsync(id);
            if (author == null)
                return NotFound();

            _mapper.Map(authorUpdateDto, author);
            await _authorRepository.UpdateAsync(author);

            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}")]
        public async Task<ActionResult> UpdatePartial(int id, [FromBody] JsonPatchDocument<AuthorUpdateDto> authorUpdateDtoPatchDoc)
        {
            Author authorFromDb = await _authorRepository.FindByIdAsync(id);
            if (authorFromDb == null)
                return NotFound();

            AuthorUpdateDto authorUpdateDto = _mapper.Map<AuthorUpdateDto>(authorFromDb);
            authorUpdateDtoPatchDoc.ApplyTo(authorUpdateDto);
            if (!TryValidateModel(authorUpdateDto))
                return ValidationProblem(ModelState);

            _mapper.Map(authorUpdateDto, authorFromDb);
            await _authorRepository.UpdateAsync(authorFromDb);

            return NoContent();
        }
    }
}
