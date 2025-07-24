using OrangeBank.Core.Domain.Entities;
using OrangeJuiceBank.Core.Domain.Enums;


namespace OrangeJuiceBank.Core.Tests.Domain.Entities
{
    public class InvestmentAccountTests
    {
        private readonly Guid _testUserId = Guid.NewGuid();
        private const string _testAccountNumber = "123456";
        private readonly Guid _testAssetId = Guid.NewGuid();
        private readonly Guid _testPortfolioId = Guid.NewGuid();

        [Fact]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            var account = new InvestmentAccount(_testUserId, _testAccountNumber);

            Assert.Equal(_testUserId, account.UserId);
            Assert.Equal(_testAccountNumber, account.AccountNumber);
            Assert.Equal(0, account.Balance);
            Assert.Equal(0, account.InvestedBalance);
            Assert.True(account.IsActive);
            Assert.Empty(account.Portfolio);
        }

        [Fact]
        public void Invest_WithPositiveAmount_IncreasesInvestedBalance()
        {
            var account = new InvestmentAccount(_testUserId, _testAccountNumber);
            account.Deposit(1000);
            decimal amount = 500;

            account.Invest(amount);

            Assert.Equal(500, account.Balance);
            Assert.Equal(500, account.InvestedBalance);
        }

        [Fact]
        public void Invest_WithZeroAmount_ThrowsException()
        {
            var account = new InvestmentAccount(_testUserId, _testAccountNumber);
            account.Deposit(1000);

            var ex = Assert.Throws<InvalidOperationException>(() => account.Invest(0));
            Assert.Equal("Investment amount must be positive", ex.Message);
        }

        [Fact]
        public void Invest_WithNegativeAmount_ThrowsException()
        {
            var account = new InvestmentAccount(_testUserId, _testAccountNumber);

            var ex = Assert.Throws<InvalidOperationException>(() => account.Invest(-100));
            Assert.Equal("Investment amount must be positive", ex.Message);
        }

        [Fact]
        public void Invest_WithInsufficientFunds_ThrowsException()
        {
            var account = new InvestmentAccount(_testUserId, _testAccountNumber);
            account.Deposit(100);

            var ex = Assert.Throws<InvalidOperationException>(() => account.Invest(200));
            Assert.Equal("Insufficient funds", ex.Message);
        }

        [Fact]
        public void ReceiveReturn_WithPositiveAmount_DecreasesInvestedBalance()
        {
            var account = new InvestmentAccount(_testUserId, _testAccountNumber);
            account.Deposit(1000);
            account.Invest(500);
            decimal returnAmount = 200;

            account.ReceiveReturn(returnAmount);

            Assert.Equal(700, account.Balance);
            Assert.Equal(300, account.InvestedBalance);
        }

        [Fact]
        public void ReceiveReturn_WithZeroAmount_ThrowsException()
        {
            var account = new InvestmentAccount(_testUserId, _testAccountNumber);

            var ex = Assert.Throws<InvalidOperationException>(() => account.ReceiveReturn(0));
            Assert.Equal("Return amount must be positive", ex.Message);
        }

        [Fact]
        public void ReceiveReturn_WithNegativeAmount_ThrowsException()
        {
            var account = new InvestmentAccount(_testUserId, _testAccountNumber);

            var ex = Assert.Throws<InvalidOperationException>(() => account.ReceiveReturn(-100));
            Assert.Equal("Return amount must be positive", ex.Message);
        }

        [Fact]
        public void Withdraw_WithNoPendingOrders_AllowsWithdrawal()
        {
            var account = new InvestmentAccount(_testUserId, _testAccountNumber);
            account.Deposit(1000);
            decimal amount = 500;

            account.Withdraw(amount);

            Assert.Equal(500, account.Balance);
        }

        [Fact]
        public void Withdraw_WithPendingOrders_ThrowsException()
        {
            var account = new InvestmentAccount(_testUserId, _testAccountNumber);
            account.Deposit(1000);

            var portfolioItem = new PortfolioItem(_testAssetId, 10, 50);
            var order = new Order(
                portfolioId: _testPortfolioId,
                type: OrderType.Buy,
                quantity: 5,
                unitPrice: 50,
                fees: 2.5m,
                tax: 0,
                assetId: _testAssetId);

            portfolioItem.AddOrder(order);
            account.Portfolio.Add(portfolioItem);

            var ex = Assert.Throws<InvalidOperationException>(() => account.Withdraw(500));
            Assert.Equal("Cannot withdraw from investment account with pending orders", ex.Message);
        }

    }
}