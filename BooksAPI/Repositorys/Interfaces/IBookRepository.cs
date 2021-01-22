using BooksAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksAPI.Repositorys.Interfaces
{
    public interface IBookRepository : ICrud<Book>
    {
        public Task<Book> FindAByIdWithoutTrackingAsync(int id);
        public Task<List<Book>> FindAllWithoutTrackingAsync();
        public Task CreateAsync(Book obj, List<Author> authors);
    }
}
