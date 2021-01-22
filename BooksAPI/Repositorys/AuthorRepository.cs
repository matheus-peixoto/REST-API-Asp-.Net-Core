﻿using BooksAPI.Data;
using BooksAPI.Models;
using BooksAPI.Repositorys.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BooksAPI.Repositorys
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly DataContext _context;

        public AuthorRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<Author> FindByIdAsync(int id) => await _context.Author.Include(a => a.AuthorsBooks).ThenInclude(ab => ab.Book).FirstOrDefaultAsync(a => a.Id == id);

        public async Task<List<Author>> FindAllAsync() => await _context.Author.Include(a => a.AuthorsBooks).ThenInclude(ab => ab.Book).ToListAsync();

        public async Task<List<Author>> FindAllWithFilterAsync(Expression<Func<Author, bool>> filter) =>
            await _context.Author.Include(a => a.AuthorsBooks).ThenInclude(ab => ab.Book).Where(filter).ToListAsync();

        public async Task CreateAsync(Author obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Author obj, List<Book> books)
        {
            obj.AuthorsBooks = new List<AuthorBook>();
            foreach (Book book in books)
            {
                book.RegisterDate = DateTime.Now;
                obj.AuthorsBooks.Add(new AuthorBook() { Author = obj, Book = book });
            }

            _context.Author.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Author obj)
        {
            _context.Author.Update(obj);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Author obj)
        {
            //Author's books ids
            int[] booksIds = obj.AuthorsBooks.Select(ab => ab.BookId).ToArray();
            //Books that has just this author
            List<Book> books = await _context.Book.Include(b => b.AuthorsBooks).Where(b => booksIds.Any(id => id == b.Id) && b.AuthorsBooks.Count == 1).ToListAsync();

            foreach (AuthorBook authorBook in obj.AuthorsBooks)
            {
                _context.Remove(authorBook);
            }

            foreach (Book book in books)
            {
                _context.Remove(book);
            }

            _context.Author.Remove(obj);

            await _context.SaveChangesAsync();
        }
    }

}
