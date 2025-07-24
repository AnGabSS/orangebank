using Microsoft.EntityFrameworkCore;
using OrangeBank.Core.Domain.Entities;
using OrangeBank.Core.Domain.Interfaces;
using OrangeBank.Infrastructure.Data;

namespace OrangeBank.Infrastructure.Repositories
{
    public class InvestmentAccountRepository : IInvestmentAccountRepository
    {

        private readonly OrangeBankContext _context;

        public InvestmentAccountRepository(OrangeBankContext context)
        {
            _context = context;
        }

        public async Task<bool> AccountNumberExists(string accountNumber)
        {
            if (await _context.InvestmentAccount.SingleOrDefaultAsync(ia => ia.AccountNumber == accountNumber) == null)
            {
                return false;
            }
            else { return true; }

        }

        public async Task AddAsync(InvestmentAccount account)
        {
            await _context.InvestmentAccount.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task<InvestmentAccount?> GetByAccountNumberAsync(string accountNumber)
        {
            return await _context.InvestmentAccount.SingleOrDefaultAsync(ia => ia.AccountNumber == accountNumber);;
        }

        public async Task<InvestmentAccount?> GetByIdAsync(Guid id)
        {
            return await _context.InvestmentAccount.SingleOrDefaultAsync(ia => ia.Id == id); ;        }

        public async Task<InvestmentAccount?> GetByUserIdAsync(Guid userId)
        {
            return await _context.InvestmentAccount.SingleOrDefaultAsync(ia => ia.UserId == userId); ;
        }

        public async Task UpdateAsync(InvestmentAccount account)
        {
            _context.InvestmentAccount.Update(account);
            await _context.SaveChangesAsync();
        }
    }
}
