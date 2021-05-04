using BooksAPI.DTOs.BookDTOs;
using BooksAPI.Repositorys.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.Libraries.Filters.Book
{
    public class ValidateBookOnCreateAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            BookCreateDto bookDto = (BookCreateDto)context.ActionArguments["bookCreateDto"];
            IAuthorRepository authoRepository = (IAuthorRepository)context.HttpContext.RequestServices.GetService(typeof(IAuthorRepository));
            if (ErrorOnAuthors(bookDto) && ErrorOnAuthorsId(bookDto))
            {
                context.ModelState
                    .AddModelError("AuthorsIds", "You need to pass the ids of that authors that wrote this book, if they are already register, or create the authors if they are not register");
                context.ModelState
                    .AddModelError("Authors", "You need to create the authors that wrote this book if they are not register, or pass the ids of the authors if they are already register");
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
            else if (!ErrorOnAuthorsId(bookDto))
            {
                if (await NotFoundedAuthorsIdAsync(bookDto, authoRepository))
                {
                    context.ModelState.AddModelError("AuthorsIds", "The passed ids does not match with any author");
                    context.Result = new BadRequestObjectResult(context.ModelState);
                }
                else
                    await next();
            }
            else
                await next();
        }

        public bool ErrorOnAuthorsId(BookCreateDto bookDto) => bookDto.AuthorsIds == null || bookDto.AuthorsIds.Length == 0;

        public bool ErrorOnAuthors(BookCreateDto bookDto) => bookDto.Authors == null || bookDto.Authors.Length == 0;

        public async Task<bool> NotFoundedAuthorsIdAsync(BookCreateDto bookDto, IAuthorRepository authorRepository)
        {
            List<Models.Author> authors = await authorRepository.FindAllWithFilterAsync(b => bookDto.AuthorsIds.Any(id => id == b.Id));
            return authors.Count == 0;
        }
    }
}
