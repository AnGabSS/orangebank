using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrangeBank.Core.Domain.Entities;

namespace OrangeBank.Core.Domain.Interfaces
{
    public interface ICheckingAccountService
    {
        Task<CheckingAccount> RegisterAscync(Guid userId);
        Task<CheckingAccount?> GetByIdAsync(Guid accountId);
        Task<CheckingAccount?> GetByUserIdAsync(Guid userId);
        Task<CheckingAccount> Deposit(Guid userId, decimal amount);
        Task<CheckingAccount> WithDraw(Guid userId, decimal amount);
    }
}
