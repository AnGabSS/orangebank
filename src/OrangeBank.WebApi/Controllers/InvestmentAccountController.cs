using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrangeBank.Application.Services;
using OrangeBank.Core.Domain.Exceptions;
using OrangeBank.Core.Domain.Interfaces;
using OrangeBank.WebApi.Models;

namespace OrangeBank.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentAccountController : ControllerBase
    {

        private InvestmentAccountService _service;

        public InvestmentAccountController(InvestmentAccountService service)
        {
            _service = service;
        }

        [HttpPost()]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterAccountRequest body)
        {
                var account = await _service.RegisterAsync(Guid.Parse(body.UserId));
                return CreatedAtAction(nameof(GetByAccountNumber), new { accountNumber = account.AccountNumber }, account);
        }

        [HttpGet("{accountNumber}")]
        public async Task<IActionResult> GetByAccountNumber(string accountNumber)
        {
            try
            {
                var account = await _service.GetByAccountNumberAsync(accountNumber);
                return Ok(account);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var account = await _service.GetByIdAsync(id);
                if (account == null)
                {
                    return NotFound("Checking account not found.");
                }
                return Ok(account);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUserIdAsync(Guid userId)
        {
            try
            {
                var account = await _service.GetByUserIdAsync(userId);
                return Ok(account);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
