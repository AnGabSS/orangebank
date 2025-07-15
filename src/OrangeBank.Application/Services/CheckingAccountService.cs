using OrangeBank.Core.Domain.Entities;
using OrangeBank.Core.Domain.Exceptions;
using OrangeBank.Core.Domain.Interfaces;

namespace OrangeBank.Application.Services
{
    public class CheckingAccountService : ICheckingAccountService
    {
        private ICheckingAccountRepository _repository;
        private IUserService _userService;

        public CheckingAccountService(ICheckingAccountRepository repository, IUserService userService)
        {
            _repository = repository;
            _userService = userService;
        }


        public async Task<CheckingAccount> RegisterAsync(Guid userId)
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

            var account = new CheckingAccount(userId, RandomNumberGenerator.GenerateRandom16DigitNumber());
            await _repository.AddAsync(account);
            return account;
        }


        public async Task<CheckingAccount?> GetByIdAsync(Guid id)
        {
            CheckingAccount account = await _repository.GetByIdAsync(id);
            return account;
        }

        public async Task<CheckingAccount> GetByUserIdAsync(Guid userId)
        {
            CheckingAccount account = await _repository.GetByUserIdAsync(userId);

            if (account == null)
            {
                throw new CheckingAccountNotFoundException("Checking account not found for the specified user ID. Please check the user ID");
            }

            return account;
        }

        public async Task<CheckingAccount> Deposit(string accountNumber, decimal amount)
        {
            CheckingAccount currentAccount = await this.GetByAccountNumberAsync(accountNumber);
            currentAccount.Deposit(amount);
            await _repository.UpdateAsync(currentAccount);
            return currentAccount;
        }

        public async Task<CheckingAccount> WithDraw(string accountNumber, decimal amount)
        {
            CheckingAccount currentAccount = await this.GetByAccountNumberAsync(accountNumber);
            currentAccount.Withdraw(amount);
            await _repository.UpdateAsync(currentAccount);
            return currentAccount;
        }

        public async Task<CheckingAccount> GetByAccountNumberAsync(string accountNumber)
        {
            CheckingAccount currentAccount = await this.GetByAccountNumberAsync(accountNumber);
            if (currentAccount == null)
            {
                throw new CheckingAccountNotFoundException("Checking account not found for the specified account number. Please check the account number");
            }
            return currentAccount;
        }
    }
}
