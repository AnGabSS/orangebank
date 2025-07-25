﻿using OrangeBank.Core.Domain.Entities;

namespace OrangeBank.Core.Domain.Interfaces
{
    public interface ICheckingAccountRepository
    {
        Task<CheckingAccount?> GetByIdAsync(Guid id);
        Task<CheckingAccount?> GetByAccountNumberAsync(string accountNumber);
        Task<Boolean> AccountNumberExists(string accountNumber);
        Task<CheckingAccount?> GetByUserIdAsync(Guid userId);
        Task AddAsync(CheckingAccount account);
        Task UpdateAsync(CheckingAccount account);
    }
}
