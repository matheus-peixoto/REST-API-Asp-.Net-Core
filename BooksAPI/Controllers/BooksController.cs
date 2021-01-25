using AutoMapper;
using BooksAPI.DTOs.BookDTOs;
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
            List<Book> booksFromDb = await _bookRepository.FindAllWithoutTrackingAsync();

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

        [HttpGet]
        [Route("{id:int}", Name = "GetBookById")]
        public async Task<ActionResult<BookReadDto>> GetById(int id)
        {
            Book bookFromDb = await _bookRepository.FindAByIdWithoutTrackingAsync(id);
            if (bookFromDb == null)
                return NotFound();

            BookReadDto bookReadDto = _mapper.Map<BookReadDto>(bookFromDb);
            Author[] authors = bookFromDb.AuthorsBooks.Select(ab => ab.Author).ToArray();
            bookReadDto.Authors = _mapper.Map<AuthorForReadBookDto[]>(authors);

            return Ok(bookReadDto);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Create([FromBody] BookCreateDto bookCreateDto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            Book book = _mapper.Map<Book>(bookCreateDto);
            book.RegisterDate = DateTime.Now;

            List<Author> authors = new List<Author>();
            bool isAuthorsNew = false;
            if (bookCreateDto.AuthorsIds != null && bookCreateDto.AuthorsIds.Length > 0)
            {
                authors = await _authorRepository.FindAllWithFilterAsync(a => bookCreateDto.AuthorsIds.Any(id => id == a.Id));
                if (authors.Count == 0)
                {
                    ModelState.AddModelError("AuthorsIds", "The passed ids does not match with any author");
                    return ValidationProblem(ModelState);
                }
            }
            else if (bookCreateDto.Authors != null && bookCreateDto.Authors.Length > 0)
            {
                authors = _mapper.Map<List<Author>>(bookCreateDto.Authors);
                isAuthorsNew = true;
            }
            else
            {
                ModelState.AddModelError("Authors", "You need to create the authors that wrote this book if they are not register, or pass the ids of the authors if they are already register");
                ModelState.AddModelError("AuthorsIds", "You need to pass the ids of that authors that wrote this book, if they are already register, or create the authors if they are not register");
                return ValidationProblem(ModelState);
            }

            book.AuthorsBooks = new List<AuthorBook>();
            foreach (Author author in authors)
            {
                if (isAuthorsNew)
                    author.RegisterDate = DateTime.Now;

                book.AuthorsBooks.Add(new AuthorBook() { Author = author, Book = book });
            }
            await _bookRepository.CreateAsync(book);
            BookReadDto bookReadDto = _mapper.Map<BookReadDto>(book);
            bookReadDto.Authors = _mapper.Map<AuthorForReadBookDto[]>(authors);

            return CreatedAtRoute("GetBookById", new { bookReadDto.Id }, bookReadDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] BookUpdateDto bookUpdateDto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            Book book = await _bookRepository.FindByIdAsync(id);
            if (book == null)
                return NotFound();

            _mapper.Map(bookUpdateDto, book);
            await _bookRepository.UpdateAsync(book);

            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}")]
        public async Task<ActionResult> UpdatePartial(int id, [FromBody] JsonPatchDocument<BookUpdateDto> bookUpdateDtoPatchDoc)
        {
            Book bookFromDb = await _bookRepository.FindByIdAsync(id);
            if (bookFromDb == null)
                return NotFound();

            BookUpdateDto bookUpdateDto = _mapper.Map<BookUpdateDto>(bookFromDb);
            bookUpdateDtoPatchDoc.ApplyTo(bookUpdateDto);
            if (!TryValidateModel(bookUpdateDto))
                return ValidationProblem(ModelState);

            _mapper.Map(bookUpdateDto, bookFromDb);
            await _bookRepository.UpdateAsync(bookFromDb);

            return NoContent();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            Book book = await _bookRepository.FindByIdAsync(id);
            if (book == null)
                return NotFound();

            await _bookRepository.DeleteAsync(book);
            return NoContent();
        }
    }
}
