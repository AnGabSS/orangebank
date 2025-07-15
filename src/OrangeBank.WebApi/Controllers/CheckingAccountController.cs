using Microsoft.AspNetCore.Mvc;
using OrangeBank.Application.Services;
using OrangeBank.Core.Domain.Exceptions;
using OrangeBank.WebApi.Models;

namespace OrangeBank.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckingAccountController : ControllerBase
{

    private CheckingAccountService _service;

    public CheckingAccountController(CheckingAccountService service)
    {
        _service = service;
    }

    [HttpPost()]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterCheckingAccountDTO body)
    {
        try
        {
            var account = await _service.RegisterAsync(Guid.Parse(body.UserId));
            return CreatedAtAction(nameof(GetByAccountNumber), new { accountNumber = account.AccountNumber }, account);
        }
        catch (ApplicationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
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


    [HttpPatch("deposit")]
    public async Task<IActionResult> DepositAsync([FromBody] FinancialMovementRequest request)
    {
        try
        {
            var account = await _service.Deposit(request.AccountNumber, request.Amount);
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

    [HttpPatch("withdraw")]
    public async Task<IActionResult> WithdrawAsync([FromBody] FinancialMovementRequest request)
    {
        try
        {
            var account = await _service.WithDraw(request.AccountNumber, request.Amount);
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

