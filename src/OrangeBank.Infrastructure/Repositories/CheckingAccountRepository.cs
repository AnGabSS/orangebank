using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrangeBank.Core.Domain.Entities;
using OrangeBank.Core.Domain.Interfaces;
using OrangeBank.Infrastructure.Data;

namespace OrangeBank.Infrastructure.Repositories
{
    public class CheckingAccountRepository : ICheckingAccountRepository
    {
        private readonly OrangeBankContext _context;

        public CheckingAccountRepository(OrangeBankContext context)
        {
            _context = context;
        }

        public async Task<bool> AccountNumberExists(string accountNumber)
        {
            if (await _context.CheckingAccounts.SingleOrDefaultAsync(ca => ca.AccountNumber == accountNumber) == null) {
                return false;
            } else { return true; }
        }

        public async Task AddAsync(CheckingAccount account)
        {
            await _context.CheckingAccounts.AddAsync(account);
        }

        public async Task<CheckingAccount?> GetByAccountNumberAync(string accountNumber)
        {
            return await _context.CheckingAccounts.SingleOrDefaultAsync(ca => ca.AccountNumber == accountNumber);
        }

        public async Task<CheckingAccount?> GetByIdAsync(Guid id)
        {
            return await _context.CheckingAccounts.FindAsync(id);
        }

        public async Task<CheckingAccount?> GetByUserIdAsync(Guid userId)
        {
            return await _context.CheckingAccounts.SingleOrDefaultAsync(ca => ca.UserId == userId);
        }

        public async Task UpdateAsync(CheckingAccount account)
        {
            _context.CheckingAccounts.Update(account);
            await _context.SaveChangesAsync();
        }
    }
}
