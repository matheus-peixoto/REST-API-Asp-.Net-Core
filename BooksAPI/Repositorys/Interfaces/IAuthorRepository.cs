using BooksAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksAPI.Repositorys.Interfaces
{
    public interface IAuthorRepository : ICrud<Author>
    {
        public Task<Author> FindAByIdWithoutTrackingAsync(int id);
        public Task<List<Author>> FindAllWithoutTrackingAsync();
    }
}
