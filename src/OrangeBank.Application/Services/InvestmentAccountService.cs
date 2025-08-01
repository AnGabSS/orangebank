using System.Security.Principal;
using OrangeBank.Core.Domain.Entities;
using OrangeBank.Core.Domain.Enums;
using OrangeBank.Core.Domain.Exceptions;
using OrangeBank.Core.Domain.Interfaces;

namespace OrangeBank.Application.Services
{
    public class InvestmentAccountService : IInvestmentAccountService
    {
        IInvestmentAccountRepository _repository;
        ICheckingAccountService _checkingAccountService;
        ITransactionService _transactionService;
        public InvestmentAccountService(
            IInvestmentAccountRepository repository, 
            ICheckingAccountService checkingAccountService, 
            ITransactionService transactionService)
        {
            _repository = repository;
            _checkingAccountService = checkingAccountService;
            _transactionService = transactionService;
        }

        public async Task<InvestmentAccount> Deposit(string accountNumber, decimal amount)
        {
            InvestmentAccount account = await GetByAccountNumberAsync(accountNumber);
            account.Withdraw(amount);
            await _repository.UpdateAsync(account);
            return account;
        }

        public async Task<InvestmentAccount> GetByAccountNumberAsync(string accountNumber)
        {
            InvestmentAccount? account = await _repository.GetByAccountNumberAsync(accountNumber);
            if (account == null) {
                throw new InvestmentAccountNotFoundException("Investment account not found for the specified account number.");
            }
            return account;
        }

        public async Task<InvestmentAccount> GetByIdAsync(Guid accountId)
        {
            InvestmentAccount? account = await _repository.GetByIdAsync(accountId);

            if (account == null)
            {
                throw new InvestmentAccountNotFoundException("Investment account not found for the specified account number.");
            }
            return account;
        }

        public async Task<InvestmentAccount> GetByUserIdAsync(Guid userId)
        {
            InvestmentAccount? account = await _repository.GetByUserIdAsync(userId);

            if (account == null)
            {
                throw new InvestmentAccountNotFoundException("Investment account not found for the specified user.");
            }

            return account;
            
        }

        public async Task<InvestmentAccount> RegisterAsync(Guid userId)
        {
            InvestmentAccount? accountExists = await GetByUserIdAsync(userId);

            if (accountExists != null)
            {
                throw new InvestmentAccountAlreadyExistsException("You already have an investment account registered for this user.");
            }
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

        public async Task<InvestmentAccount> WithDraw(string accountNumber, decimal amount)
        {
            InvestmentAccount? investmentAccount = await GetByAccountNumberAsync(accountNumber);
            investmentAccount.Withdraw(amount);
            await _repository.UpdateAsync(investmentAccount);
            return investmentAccount;
        }

        public async Task<InvestmentAccount> WithDrawByUserId(Guid userId, decimal amount)
        {
            InvestmentAccount? investmentAccount = await GetByUserIdAsync(userId);
            investmentAccount.Withdraw(amount);
            await _repository.UpdateAsync(investmentAccount);
            return investmentAccount;
        }

        public async Task<InvestmentAccount> TransferBalanceToCheckingAccount(Guid userId, decimal amount)
        {

            // Update Investment Account amount
            await WithDraw(userId, amount);

            // Update Checking Account amount
            await _checkingAccountService.WithDrawByUserId(userId, amount);

            // Register the transaction
            Transaction transaction = new Transaction(
                investmentAccount,
                checkingAccount,
                amount,
                TransactionType.INTERNAL);

            await _transactionService.AddAsync(transaction);

            return investmentAccount;

        }


    }
}
