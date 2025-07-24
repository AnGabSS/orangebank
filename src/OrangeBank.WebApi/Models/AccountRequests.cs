namespace OrangeBank.WebApi.Models;

public class FinancialMovementRequest
{
    public required decimal Amount { get; set; }
    public required string AccountNumber { get; set; } = string.Empty;
}

public class RegisterAccountRequest
{
    public required string UserId { get; set; }
}
