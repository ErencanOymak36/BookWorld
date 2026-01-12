using AutoMapper;
using BookWorld.Application.DTOs;
using BookWorld.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookWorld.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly IMapper _mapper;
        public BookController(BookService bookService,IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpPost("CreateBook")]
        public async Task<IActionResult> CreateBook(CreateBookDto dto)
        {
            await _bookService.CreateBookAsync(dto);
            return Ok("Book Created");
        }
        [HttpPost("UpdateBook")]
        public async Task<IActionResult> UpdateBook(UpdateBookDto dto)
        {
            await _bookService.UpdateBookAsync(dto);
            return Ok("Book Updated");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var result=await _bookService.GetBookByIdAsync(id);
            return Ok(result);
        }

        
        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            var result = await _bookService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("by-author")]
        public async Task<IActionResult> GetBooksByAuthor([FromQuery] string authorName)
        {
            if (string.IsNullOrWhiteSpace(authorName))
                return BadRequest("Author name is required");

            var result = await _bookService.GetBookByAuthorAsync(authorName);
            return Ok(result);
        }

    }
}
