using BooksAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Author> Author { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<AuthorBook> AuthorBook { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}
