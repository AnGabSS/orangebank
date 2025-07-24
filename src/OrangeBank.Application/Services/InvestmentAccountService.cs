using OrangeBank.Core.Domain.Entities;
using OrangeBank.Core.Domain.Interfaces;

namespace OrangeBank.Application.Services
{
    public class InvestmentAccountService : IInvestmentAccountService
    {
        IInvestmentAccountRepository _repository;
        public InvestmentAccountService(IInvestmentAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<InvestmentAccount?> GetByAccountNumberAsync(string accountNumber)
        {
            return await _repository.GetByAccountNumberAsync(accountNumber);
        }

        public async Task<InvestmentAccount?> GetByIdAsync(Guid accountId)
        {
            return await _repository.GetByIdAsync(accountId);
        }

        public async Task<InvestmentAccount?> GetByUserIdAsync(Guid userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        public async Task<InvestmentAccount> RegisterAsync(Guid userId)
        {
            string randomAccountNumber = RandomNumberGenerator.GenerateRandom16DigitNumber();

            int generateAccountNumberAttempts = 0;

            // Maximum number of retries to find a unique account number.
            // The first generation attempt is not counted as a retry.
            const int MAX_ACCOUNT_NUMBER_GENERATION_RETRIES = 9;

            // Generate a new random number while checking if it already exists
            while (await _repository.AccountNumberExists(randomAccountNumber) && generateAccountNumberAttempts < MAX_ACCOUNT_NUMBER_GENERATION_RETRIES)
            {
                randomAccountNumber = RandomNumberGenerator.GenerateRandom16DigitNumber();
                generateAccountNumberAttempts++;
            }

            // If we reach the maximum attempts without finding a unique account number, throw an exception, cause the accountNumber must to be UNIQUE
            if (generateAccountNumberAttempts == MAX_ACCOUNT_NUMBER_GENERATION_RETRIES)
            {
                throw new ApplicationException("Checking Account creation failed. Please try again in a few moments.");
            }

            InvestmentAccount account = new InvestmentAccount(userId, randomAccountNumber);
            await _repository.AddAsync(account);
            return account;
        }
    }
}
