using BookWorld.Application.DTOs;
using BookWorld.Application.Interfaces;
using BookWorld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Application.Services
{
    public class BookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task CreateBookAsync(CreateBookDto dto)
        {
            Book newBook = new Book
            {
                Title = dto.Title,
                ISBN = dto.ISBN,
                Price = dto.Price,
                Stock = dto.Stock,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                AuthorId = dto.AuthorId,
                CategoryId = dto.CategoryId
            };

            await _bookRepository.CreateBookAsync(newBook);
        }

        //public async Task UpdateBookAsync(BookUpdateDto dto)
        //{
        //    Book book = new Book
        //    {
        //        Id = dto.Id,
        //        Title = dto.Title,
        //        ISBN = dto.ISBN,
        //        Price = dto.Price,
        //        Stock = dto.Stock,
        //        Description = dto.Description,
        //        ImageUrl = dto.ImageUrl,
        //        AuthorId = dto.AuthorId,
        //        CategoryId = dto.CategoryId
        //    };

        //    await _bookRepository.UpdateBookAsync(book);
        //}

        public async Task DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);

            if (book is null)
                return;

            await _bookRepository.DeleteBookAsync(book);
        }

        public async Task<BookDto> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);

            if (book is null)
                return null;

            return ConvertToDto(book);
        }

        public async Task<List<BookDto>> GetAllAsync()
        {
            var books = await _bookRepository.GetAllBooksAsync();

            return books.Select(x => ConvertToDto(x)).ToList();
        }

        private BookDto ConvertToDto(Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                Price = book.Price,
                Stock = book.Stock,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                AuthorName = book.Author?.Name,
                CategoryName = book.Category?.Name
            };
        }
    }
}
