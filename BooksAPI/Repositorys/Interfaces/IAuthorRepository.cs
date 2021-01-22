﻿using BooksAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksAPI.Repositorys.Interfaces
{
    public interface IAuthorRepository
    {
        public Task CreateAsync(Author obj, List<Book> books);
    }
}
