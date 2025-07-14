using Microsoft.AspNetCore.Mvc;
using OrangeBank.Application.Services;
using OrangeBank.Core.Domain.Interfaces;
using OrangeBank.Core.Domain.Security;
using OrangeBank.WebApi.Models;

namespace OrangeBank.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _userService.AuthenticateUserAsync(loginDto.Email, loginDto.Password);
                return Ok(new { Token = token });
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.Contains("credentials", StringComparison.OrdinalIgnoreCase) ||
                    ex.Message.Contains("senha", StringComparison.OrdinalIgnoreCase) ||
                    ex.Message.Contains("password", StringComparison.OrdinalIgnoreCase))
                {
                    return Unauthorized(new { Message = "Invalid Credencials" });
                }

                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro durante o login");
                return StatusCode(500, new { Message = "Ocorreu um erro interno" });
            }
        }
    }
}