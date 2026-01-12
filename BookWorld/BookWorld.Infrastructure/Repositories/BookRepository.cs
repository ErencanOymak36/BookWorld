using BookWorld.Application.Interfaces;
using BookWorld.Domain.Entities;
using BookWorld.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookWorldDbContext _context;
        public BookRepository(BookWorldDbContext context)
        {
            _context = context;
        }

        public async Task CreateBookAsync(Book book)
        {
            await _context.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            //return await _context.Books.OrderByDescending(b=>b.Id).ToListAsync();
            return await _context.Books.Include(b => b.Author).Include(b => b.Category).ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
           return await _context.Books.FindAsync(id);
        }
        public async Task<IEnumerable<Book>> GetBookByAuthorAsync(string authorName)
        {
            return await _context.Books.Include(b=>b.Author).Where(b=>b.Author.Name.Contains(authorName)).ToListAsync();
        }


        public async Task UpdateBookAsync(Book book)
        {
             _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        
    }
}
