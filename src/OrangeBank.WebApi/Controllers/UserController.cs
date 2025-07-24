using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrangeBank.Application.Services;
using OrangeBank.Core.Domain.Interfaces;
using OrangeBank.WebApi.Models;

namespace OrangeBank.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        try
        {
            var user = await _userService.RegisterUserAsync(
                request.Name,
                request.Email,
                request.Cpf,
                request.PhoneNumber,
                request.BirthDate,
                request.Password
            );

            return CreatedAtAction(nameof(GetById), new { userId = user.Id }, user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetById(Guid userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null) return NotFound();

        return Ok(user);
    }

    [HttpPost("{userId:guid}/change-password")]
    public async Task<IActionResult> ChangePassword(Guid userId, [FromBody] ChangePasswordRequest request)
    {
        try
        {
            await _userService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("request-password-reset")]
    public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequest request)
    {
        await _userService.RequestPasswordResetAsync(request.Email);
        return NoContent(); // Mesmo que o e-mail não exista, retorna 204
    }
}
