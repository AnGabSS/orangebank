using OrangeBank.Core.Domain.Entities;

namespace OrangeBank.Core.Domain.Interfaces
{
    public interface ICheckingAccountService
    {
        Task<CheckingAccount> RegisterAsync(Guid userId);
        Task<CheckingAccount> GetByIdAsync(Guid accountId);
        Task<CheckingAccount> GetByUserIdAsync(Guid userId);
        Task<CheckingAccount> GetByAccountNumberAsync(string accountNumber);
        Task<CheckingAccount> Deposit(string accountNumber, decimal amount);
        Task<CheckingAccount> WithDraw(string accountNumber, decimal amount);
        Task<CheckingAccount> WithDrawByUserId(Guid userId, decimal amount);
    }
}
