using Xunit;
using FluentAssertions;
using OrangeBank.Core.Domain.Entities;
using System;

namespace OrangeBank.Core.Domain.Tests.Entities
{
    public class CheckingAccountTests
    {
        private readonly Guid _testUserId = Guid.NewGuid();
        private const string _testAccountNumber = "12345-6";
        private const decimal DailyWithdrawalLimit = 5000m; // Acessível para testes

        [Fact]
        public void CheckingAccount_Constructor_InitializesCorrectly()
        {
            // Arrange & Act
            var account = new CheckingAccount(_testUserId, _testAccountNumber);

            // Assert
            account.Id.Should().NotBeEmpty();
            account.UserId.Should().Be(_testUserId);
            account.AccountNumber.Should().Be(_testAccountNumber);
            account.Balance.Should().Be(0);
            account.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            account.IsActive.Should().BeTrue();
            account.DailyWithdrawn.Should().Be(0);
            account.LastWithdrawalDate.Should().Be(DateTime.UtcNow.Date);
        }

        [Fact]
        public void Deposit_ShouldIncreaseBalance()
        {
            var account = new CheckingAccount(_testUserId, _testAccountNumber);
            account.Deposit(100);

            account.Deposit(50);

            account.Balance.Should().Be(150);
        }

        [Fact]
        public void Deposit_ZeroAmount_ShouldThrowInvalidOperationException()
        {
            var account = new CheckingAccount(_testUserId, _testAccountNumber);

            Action act = () => account.Deposit(0);

            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Deposit amount must be positive");
        }

        [Fact]
        public void Deposit_NegativeAmount_ShouldThrowInvalidOperationException()
        {
            var account = new CheckingAccount(_testUserId, _testAccountNumber);

            Action act = () => account.Deposit(-10);

            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Deposit amount must be positive");
        }

        [Fact]
        public void Withdraw_ShouldDecreaseBalanceWithinLimit()
        {
            var account = new CheckingAccount(_testUserId, _testAccountNumber);
            account.Deposit(1000);

            account.Withdraw(500);

            account.Balance.Should().Be(500);
            account.DailyWithdrawn.Should().Be(500);
            account.LastWithdrawalDate.Should().Be(DateTime.UtcNow.Date);
        }

        [Fact]
        public void Withdraw_MultipleTimes_ShouldAccumulateDailyWithdrawn()
        {
            var account = new CheckingAccount(_testUserId, _testAccountNumber);
            account.Deposit(2000);

            account.Withdraw(500);
            account.Withdraw(700);

            account.Balance.Should().Be(800);
            account.DailyWithdrawn.Should().Be(1200);
            account.LastWithdrawalDate.Should().Be(DateTime.UtcNow.Date);
        }

        [Fact]
        public void Withdraw_ExceedingBalance_ShouldThrowInvalidOperationException()
        {
            var account = new CheckingAccount(_testUserId, _testAccountNumber);
            account.Deposit(100);

            Action act = () => account.Withdraw(200);

            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Insufficient funds");
            account.Balance.Should().Be(100);
            account.DailyWithdrawn.Should().Be(0);
        }

        [Fact]
        public void Withdraw_ZeroAmount_ShouldThrowInvalidOperationException()
        {
            var account = new CheckingAccount(_testUserId, _testAccountNumber);
            account.Deposit(100);

            Action act = () => account.Withdraw(0);

            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Withdrawal amount must be positive");
        }

        [Fact]
        public void Withdraw_NegativeAmount_ShouldThrowInvalidOperationException()
        {
            var account = new CheckingAccount(_testUserId, _testAccountNumber);
            account.Deposit(100);

            Action act = () => account.Withdraw(-10);

            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Withdrawal amount must be positive");
        }

        [Fact]
        public void Withdraw_ExceedingDailyLimitInOneGo_ShouldThrowInvalidOperationException()
        {
            var account = new CheckingAccount(_testUserId, _testAccountNumber);
            account.Deposit(10000);

            Action act = () => account.Withdraw(DailyWithdrawalLimit + 1);

            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Withdrawal limit exceeded");
            account.Balance.Should().Be(10000);
            account.DailyWithdrawn.Should().Be(0);
        }

        [Fact]
        public void Withdraw_ExceedingDailyLimitAcrossMultipleWithdrawals_ShouldThrowInvalidOperationException()
        {
            var account = new CheckingAccount(_testUserId, _testAccountNumber);
            account.Deposit(10000);

            account.Withdraw(DailyWithdrawalLimit - 1000); // Ex: 4000

            Action act = () => account.Withdraw(1500); // Ex: 4000 + 1500 = 5500

            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Withdrawal limit exceeded");
            account.Balance.Should().Be(10000 - (DailyWithdrawalLimit - 1000));
            account.DailyWithdrawn.Should().Be(DailyWithdrawalLimit - 1000);
        }

        [Fact]
        public void Withdraw_DailyLimitResetsOnNewDay()
        {
            var account = new CheckingAccount(_testUserId, _testAccountNumber);
            account.Deposit(15000);

            account.Withdraw(3000); // Saque no "dia 1"
            account.DailyWithdrawn.Should().Be(3000);
            account.LastWithdrawalDate.Should().Be(DateTime.UtcNow.Date);

            // Simula que a data da última retirada é de um dia anterior.
            // Isso geralmente seria feito com um mock de IDateTimeProvider.
            // Para este teste unitário, usamos reflexão para forçar o estado para demonstração.
            typeof(CheckingAccount)
                .GetProperty(nameof(CheckingAccount.LastWithdrawalDate))
                .SetValue(account, DateTime.UtcNow.Date.AddDays(-1));
            typeof(CheckingAccount)
                .GetProperty(nameof(CheckingAccount.DailyWithdrawn))
                .SetValue(account, 4000m);

            // Atuamos no "novo dia"
            account.Withdraw(4500);

            account.Balance.Should().Be(15000 - 3000 - 4500);
            account.DailyWithdrawn.Should().Be(4500);
            account.LastWithdrawalDate.Should().Be(DateTime.UtcNow.Date);
        }
    }
}