using AutoMapper;
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
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task CreateBookAsync(CreateBookDto dto)
        {
            var entity = _mapper.Map<Book>(dto);
            await _bookRepository.CreateBookAsync(entity);
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
            if (book != null)
                await _bookRepository.DeleteBookAsync(book);
        }

        public async Task<BookDto> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<List<BookDto>> GetAllAsync()
        {
            var books = await _bookRepository.GetAllBooksAsync();
            return _mapper.Map<List<BookDto>>(books);
        }

        //private BookDto ConvertToDto(Book book)
        //{
        //    return new BookDto
        //    {
        //        Id = book.Id,
        //        Title = book.Title,
        //        ISBN = book.ISBN,
        //        Price = book.Price,
        //        Stock = book.Stock,
        //        Description = book.Description,
        //        ImageUrl = book.ImageUrl,
        //        AuthorName = book.Author?.Name,
        //        CategoryName = book.Category?.Name
        //    };
        //}
    }
}
