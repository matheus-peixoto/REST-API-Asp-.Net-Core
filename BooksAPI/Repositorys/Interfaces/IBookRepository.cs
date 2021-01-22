using BooksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.Repositorys.Interfaces
{
    public interface IBookRepository : ICrud<Book>
    {
        public Task CreateAsync(Book obj, List<Author> authors);
    }
}
