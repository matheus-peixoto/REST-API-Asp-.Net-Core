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
            AuthorCreateDto authorDto = (AuthorCreateDto)context.ActionArguments["authorCreateDto"];
            IBookRepository bookRepository = (IBookRepository)context.HttpContext.RequestServices.GetService(typeof(IBookRepository));

            if (ErrorOnBooks(authorDto) && ErrorOnBooksIds(authorDto))
            {
                context.ModelState.AddModelError("BooksIds", "You need to pass the ids of the books that this author wrote or create the books that this author wrote");
                context.ModelState.AddModelError("Books", "You need to create the books that this author wrote or pass the ids of the books that this author wrote");
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
            else if (!ErrorOnBooksIds(authorDto))
            {
                if (await NotFoundedBooksIdsAsync(authorDto, bookRepository))
                {
                    context.ModelState.AddModelError("BooksIds", "The passed ids does not match with any book. You n");
                    context.Result = new BadRequestObjectResult(context.ModelState);
                }
                else await next();       
            }
            else
                await next();
        }

        private bool ErrorOnBooksIds(AuthorCreateDto authorDto) => authorDto.BooksIds is null || authorDto.BooksIds.Length == 0;

        private bool ErrorOnBooks(AuthorCreateDto authorDto) => authorDto.Books is null || authorDto.Books.Length == 0;

        private async Task<bool> NotFoundedBooksIdsAsync(AuthorCreateDto authorDto, IBookRepository bookRepository)
        {
            List<Book> books = await bookRepository.FindAllWithFilterAsync(b => authorDto.BooksIds.Any(id => id == b.Id));
            return books.Count == 0;
        }
    }
}
