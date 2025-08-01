using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrangeBank.Core.Domain.Entities;
using OrangeBank.Core.Domain.Interfaces;

namespace OrangeBank.Application.Services;

public class TransactionService : ITransactionService
{

    private ITransactionRepository _repository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _repository = transactionRepository;
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _repository.AddAsync(transaction);
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IEnumerable<Transaction>> GetByAccountAsync(string accountNumber)
    {
        return await _repository.GetByAccountAsync(accountNumber);
    }

    public async Task<IEnumerable<Transaction>> GetByDestinyAccountAsync(string destinyAccount)
    {
        return await _repository.GetByDestinyAccountAsync(destinyAccount);
    }

    public async Task<Transaction?> GetByIdAsync(Guid transactionId)
    {
        return await _repository.GetByIdAsync(transactionId);
    }

    public async Task<IEnumerable<Transaction>> GetByOriginAccountAsync(string originAccount)
    {
        return await _repository.GetByOriginAccountAsync(originAccount);
    }

    public async Task<IEnumerable<Transaction>> GetByUserAsync(Guid userId)
    {
        return await _repository.GetByUserAsync(userId);
    }
}

