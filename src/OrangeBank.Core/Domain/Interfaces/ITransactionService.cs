
using OrangeBank.Core.Domain.Entities;

namespace OrangeBank.Core.Domain.Interfaces;

public interface ITransactionService
{
    Task AddAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetByAccountAsync(string accountNumber);
    Task<IEnumerable<Transaction>> GetByUserAsync(Guid userId);
    Task<IEnumerable<Transaction>> GetByOriginAccountAsync(string originAccount);
    Task<IEnumerable<Transaction>> GetByDestinyAccountAsync(string destinyAccount);
    Task<IEnumerable<Transaction>> GetAllAsync();
    Task<Transaction?> GetByIdAsync(Guid transactionId);

}

