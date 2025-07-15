namespace OrangeBank.WebApi.Models;

public class FinancialMovementRequest
{
    public decimal Amount { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
}
