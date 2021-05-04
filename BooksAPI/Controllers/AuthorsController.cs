﻿using AutoMapper;
using BooksAPI.DTOs.AuthorDTOs;
using BooksAPI.Libraries.Filters.Author;
using BooksAPI.Libraries.Services;
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
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly AuthorsControllerServices _controllerServices;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepository authorRepository, IBookRepository bookRepository, AuthorsControllerServices controllerServices, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _controllerServices = controllerServices;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Get()
        {
            List<Author> authorsFromDb = await _authorRepository.FindAllWithoutTrackingAsync();
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
            Author authorFromDb = await _authorRepository.FindAByIdWithoutTrackingAsync(id);
            if (authorFromDb is null) return NotFound();

            AuthorReadDto authorReadDto = _mapper.Map<AuthorReadDto>(authorFromDb);
            Book[] books = authorFromDb.AuthorsBooks.Select(ab => ab.Book).ToArray();
            authorReadDto.Books = _mapper.Map<BookForReadAuthorDto[]>(books);
            return Ok(authorReadDto);
        }

        [HttpPost]
        [Route("")]
        [ValidateAuthorOnCreate]
        public async Task<ActionResult> Create([FromBody] AuthorCreateDto authorCreateDto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            Author author = await _controllerServices.FilledOutAuthorOnCreateAsync(authorCreateDto);
            await _authorRepository.CreateAsync(author);
            AuthorReadDto authorReadDto = _controllerServices.GetAuthorReadDtoOnCreate(author);

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

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            Author author = await _authorRepository.FindByIdAsync(id);
            if (author == null)
                return NotFound();

            await _authorRepository.DeleteAsync(author);
            return NoContent();
        }
    }
}
