using BookWorld.Application.DTOs;
using BookWorld.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookWorld.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentalController : ControllerBase
    {
        private readonly RentalService _rentalService;

        public RentalController(RentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpPost("CreateRental")]
        public async Task<IActionResult> CreateRental(CreateRentalDto dto)
        {
            await _rentalService.CreateRentalAsync(dto);
            return Ok("Rental created");
        }

        [HttpGet("GetAllRentals")]
        public async Task<IActionResult> GetAllRentals()
        {
            var result = await _rentalService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _rentalService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("getbyuser/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var result = await _rentalService.GetByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpPut("returnbook/{id}")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            await _rentalService.ReturnRentalAsync(id);
            return Ok("Book returned");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _rentalService.DeleteAsync(id);
            return Ok("Rental deleted");
        }

        [HttpGet("active")]
        public async Task<IActionResult> ActiveRentals()
        {
            return Ok(await _rentalService.GetActiveRentalsAsync());
        }

        [HttpGet("late")]
        public async Task<IActionResult> LateRentals()
        {
            return Ok(await _rentalService.GetLateRentalsAsync());
        }

        [HttpPost("CancelRental")]
        public async Task<IActionResult> CancelRental(int id)
        {
            return Ok(await _rentalService.CancelRentalManuelAsync(id));
        }


        [HttpGet("Completed")]
        public async Task<IActionResult> CompletedRentals()
        {
            return Ok(await _rentalService.GetCompletedRentalsAsync());
        }
    }
}
