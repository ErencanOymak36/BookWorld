using BookWorld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Application.Interfaces
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author> GetByIdAsync(int id);
      //  Task<IEnumerable<Author>> GetByNameAsync(string name);
        Task CreateAsync(Author author);
        Task UpdateAsync(Author author);
        Task<bool> DeleteAsync(int id);
    }
}
