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
            if (await _context.CheckingAccount.SingleOrDefaultAsync(ca => ca.AccountNumber == accountNumber) == null) {
                return false;
            } else { return true; }
        }

        public async Task AddAsync(CheckingAccount account)
        {
            await _context.CheckingAccount.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task<CheckingAccount?> GetByAccountNumberAsync(string accountNumber)
        {
            return await _context.CheckingAccount.SingleOrDefaultAsync(ca => ca.AccountNumber == accountNumber);
        }

        public async Task<CheckingAccount?> GetByIdAsync(Guid id)
        {
            return await _context.CheckingAccount.FindAsync(id);
        }

        public async Task<CheckingAccount?> GetByUserIdAsync(Guid userId)
        {
            return await _context.CheckingAccount.SingleOrDefaultAsync(ca => ca.UserId == userId);
        }

        public async Task UpdateAsync(CheckingAccount account)
        {
            _context.CheckingAccount.Update(account);
            await _context.SaveChangesAsync();
        }
    }
}
