using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrangeBank.Core.Domain.Entities;

namespace OrangeBank.Core.Domain.Interfaces
{
    public interface IInvestmentAccountService
    {
        Task<InvestmentAccount> RegisterAsync(Guid userId);
        Task<InvestmentAccount?> GetByIdAsync(Guid accountId);
        Task<InvestmentAccount?> GetByUserIdAsync(Guid userId);
        Task<InvestmentAccount?> GetByAccountNumberAsync(string accountNumber);
    }
}
