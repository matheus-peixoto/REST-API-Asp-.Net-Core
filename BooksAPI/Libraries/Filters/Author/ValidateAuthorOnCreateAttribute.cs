using AutoMapper;
using BooksAPI.DTOs.AuthorDTOs;
using BooksAPI.Models;
using BooksAPI.Repositorys.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.Libraries.Filters.Author
{
    public class ValidateAuthorOnCreateAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IMapper mapper = (IMapper)context.HttpContext.RequestServices.GetService(typeof(IMapper));
            AuthorCreateDto authorCreateDto = (AuthorCreateDto)context.ActionArguments["authorCreateDto"];
            IBookRepository bookRepository = (IBookRepository)context.HttpContext.RequestServices.GetService(typeof(IBookRepository));
            Models.Author author = mapper.Map<Models.Author>(authorCreateDto);

            List<Book> books = await bookRepository.FindAllWithFilterAsync(b => authorCreateDto.BooksIds.Any(id => id == b.Id));
            if (authorCreateDto.BooksIds != null && authorCreateDto.BooksIds.Length > 0 && books.Count == 0)
            {
                context.ModelState.AddModelError("BooksIds", "The passed ids does not match with any book");
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
            else if (authorCreateDto.Books is null || authorCreateDto.Books.Length == 0)
            {
                context.ModelState.AddModelError("BooksIds", "You need to pass the ids of the books that this author wrote or create the books that this author wrote");
                context.ModelState.AddModelError("Books", "You need to create the books that this author wrote or pass the ids of the books that this author wrote");
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
            else
                await next();
        }
    }
}
