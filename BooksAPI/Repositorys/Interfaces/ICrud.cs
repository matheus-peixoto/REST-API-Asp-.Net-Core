using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BooksAPI.Repositorys.Interfaces
{
    public interface ICrud<T> where T : class
    {
        public Task<T> FindByIdAsync(int id);
        public Task<List<T>> FindAllAsync();
        public Task<List<T>> FindAllWithFilterAsync(Expression<Func<T, bool>> filter);

        public Task CreateAsync(T obj);
        public Task UpdateAsync(T obj);
        public Task DeleteAsync(T obj);
    }
}
