using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrangeBank.Core.Domain.Entities;

namespace OrangeBank.Core.Domain.Interfaces
{
    public interface ICheckingAccountRepository
    {
        Task<CheckingAccount?> GetByIdAsync(Guid id);
        Task<CheckingAccount[]?> GetAllByUserIdAsync(Guid userId);
        Task AddAsync(CheckingAccount user);
        Task UpdateAsync(CheckingAccount user);
    }
}
