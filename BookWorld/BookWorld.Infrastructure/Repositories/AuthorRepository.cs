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
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BookWorldDbContext _context;

        public AuthorRepository(BookWorldDbContext context)
        {
            _context=context;
        }
        public async Task CreateAsync(Author author)
        {
            await _context.AddAsync(author);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Authors.FindAsync(id);
            if (entity == null)
                return false;

            _context.Authors.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors.OrderByDescending(b => b.Id).ToListAsync();
        }

        public async Task<Author> GetByIdAsync(int id)
        {
            return await _context.Authors.Include(x => x.Books).FirstOrDefaultAsync(x => x.Id == id);
        }

        //public async Task<IEnumerable<Author>> GetByNameAsync(string name)
        //{
        //    return await _context.Authors.FirstOrDefaultAsync(a => a.Name == name);
        //}

        public async Task UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
        }
    }
}
