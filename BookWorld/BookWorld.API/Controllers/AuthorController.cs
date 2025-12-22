using BookWorld.Application.DTOs;
using BookWorld.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookWorld.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _authorService;
        public AuthorController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAuthors()
        {
            var result = await _authorService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _authorService.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        [HttpPost("CreateAuthor")]
        public async Task<IActionResult> CreateAuthor(CreateAuthorDto dto)
        {
            await _authorService.CreateAsync(dto);
            return Ok("Author created");
        }

        [HttpPost("UpdateAuthor")]
        public async Task<IActionResult> UpdateAuthor(UpdateAuthorDto dto)
        {
            await _authorService.UpdateAsync(dto);
            return Ok("Author updated");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _authorService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return Ok("Author deleted");
        }
    }
}
