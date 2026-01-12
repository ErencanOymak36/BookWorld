using BookWorld.Application.DTOs;
using BookWorld.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookWorld.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthController(
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
                return BadRequest("Login bilgileri boş");

            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null)
                return Unauthorized("Kullanıcı Bulunamadı");

            if (user.PasswordHash != loginDto.Password)
                return Unauthorized("Şifre Hatalı");

            var token = _tokenService.CreateToken(
                user.Id,
                user.Email,
                user.Role
            );

            return Ok(new LoginResponseDto
            {
                AccessToken = token
            });
        }

    }
}
