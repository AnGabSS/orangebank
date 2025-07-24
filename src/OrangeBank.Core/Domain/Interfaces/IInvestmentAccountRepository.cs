using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrangeBank.Core.Domain.Entities;

namespace OrangeBank.Core.Domain.Interfaces
{
    public interface IInvestmentAccountRepository
    {       
        Task<InvestmentAccount?> GetByIdAsync(Guid id);
        Task<InvestmentAccount?> GetByAccountNumberAsync(string accountNumber);
        Task<Boolean> AccountNumberExists(string accountNumber);
        Task<InvestmentAccount?> GetByUserIdAsync(Guid userId);
        Task AddAsync(InvestmentAccount account);
        Task UpdateAsync(InvestmentAccount account);
    }
}

