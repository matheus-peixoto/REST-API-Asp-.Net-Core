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
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _context;

        public BookRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<Book> FindByIdAsync(int id) => await _context.Book.Include(b => b.AuthorsBooks).ThenInclude(ab => ab.Author).FirstOrDefaultAsync(a => a.Id == id);

        public async Task<Book> FindAByIdWithoutTrackingAsync(int id) => await _context.Book.AsNoTracking().Include(b => b.AuthorsBooks).ThenInclude(ab => ab.Author).FirstOrDefaultAsync(a => a.Id == id);

        public async Task<List<Book>> FindAllAsync() => await _context.Book.Include(b => b.AuthorsBooks).ThenInclude(ab => ab.Author).ToListAsync();

        public async Task<List<Book>> FindAllWithoutTrackingAsync() => await _context.Book.AsNoTracking().Include(b => b.AuthorsBooks).ThenInclude(ab => ab.Author).ToListAsync();

        public async Task<List<Book>> FindAllWithFilterAsync(Expression<Func<Book, bool>> filter) =>
            await _context.Book.Include(b => b.AuthorsBooks).ThenInclude(ab => ab.Author).Where(filter).ToListAsync();

        public async Task CreateAsync(Book obj)
        {
            _context.Book.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book obj)
        {
            _context.Book.Update(obj);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Book obj)
        {
            //Book's authors ids
            int[] authorsIds = obj.AuthorsBooks.Select(ab => ab.AuthorId).ToArray();
            //Authors that just write this book
            List<Author> authors = await _context.Author.Include(a => a.AuthorsBooks).Where(a => authorsIds.Any(id => id == a.Id) && a.AuthorsBooks.Count == 1).ToListAsync();

            _context.AuthorBook.RemoveRange(obj.AuthorsBooks);

            _context.Author.RemoveRange(authors);

            _context.Book.Remove(obj);

            await _context.SaveChangesAsync();
        }
    }
}
