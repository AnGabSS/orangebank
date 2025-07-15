using Moq;
using OrangeBank.Application.Services;
using OrangeBank.Core.Domain.Entities;
using OrangeBank.Core.Domain.Interfaces;
using OrangeBank.Core.Domain.Exceptions;
using System.Reflection; 

namespace OrangeBank.Application.Tests.Services
{
    public class CheckingAccountServiceTests
    {
        private readonly Mock<ICheckingAccountRepository> _mockRepository;
        private readonly Mock<IUserService> _mockUserService;
        private readonly CheckingAccountService _service;
        private readonly Guid _testUserId = Guid.NewGuid();
        private readonly Guid _testAccountId = Guid.NewGuid();
        private const string _mockAccountNumber = "1234567890123456"; 

        public CheckingAccountServiceTests()
        {
            _mockRepository = new Mock<ICheckingAccountRepository>();
            _mockUserService = new Mock<IUserService>();
            _service = new CheckingAccountService(_mockRepository.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task RegisterAsync_ShouldCreateNewAccount_WhenAccountNumberIsUnique()
        {

            _mockRepository.Setup(r => r.AccountNumberExists(It.IsAny<string>()))
                           .ReturnsAsync(false); 


            var account = await _service.RegisterAsync(_testUserId);


            Assert.NotNull(account);
            Assert.Equal(_testUserId, account.UserId);

            _mockRepository.Verify(r => r.AccountNumberExists(It.IsAny<string>()), Times.Once);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<CheckingAccount>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldRetryGeneratingAccountNumber_WhenCollisionOccurs()
        {

            _mockRepository.SetupSequence(r => r.AccountNumberExists(It.IsAny<string>()))
                           .ReturnsAsync(true)  
                           .ReturnsAsync(false); 

            var account = await _service.RegisterAsync(_testUserId);

            Assert.NotNull(account);

            _mockRepository.Verify(r => r.AccountNumberExists(It.IsAny<string>()), Times.Exactly(2));
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<CheckingAccount>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenMaxAttemptsReached()
        {
            _mockRepository.Setup(r => r.AccountNumberExists(It.IsAny<string>()))
                           .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<ApplicationException>(
                async () => await _service.RegisterAsync(_testUserId));

            Assert.Equal("Checking Account creation failed. Please try again in a few moments.", exception.Message);

            _mockRepository.Verify(r => r.AccountNumberExists(It.IsAny<string>()), Times.Exactly(10));
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<CheckingAccount>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAccount_WhenFound()
        {
            var expectedAccount = new CheckingAccount(_testUserId, _mockAccountNumber);
            typeof(CheckingAccount).GetProperty(nameof(CheckingAccount.Id), BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                                   .SetValue(expectedAccount, _testAccountId);

            _mockRepository.Setup(r => r.GetByIdAsync(_testAccountId))
                           .ReturnsAsync(expectedAccount);

            var result = await _service.GetByIdAsync(_testAccountId);

            Assert.NotNull(result);
            Assert.Equal(expectedAccount.Id, result.Id);
            Assert.Equal(expectedAccount.UserId, result.UserId);
            Assert.Equal(expectedAccount.AccountNumber, result.AccountNumber);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {       
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync((CheckingAccount)null);

            var result = await _service.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnAccount_WhenFound()
        {
            var expectedAccount = new CheckingAccount(_testUserId, _mockAccountNumber);
            _mockRepository.Setup(r => r.GetByUserIdAsync(_testUserId))
                           .ReturnsAsync(expectedAccount);

            var result = await _service.GetByUserIdAsync(_testUserId);

            Assert.NotNull(result);
            Assert.Equal(expectedAccount.UserId, result.UserId);
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldThrowCheckingAccountNotFoundException_WhenNotFound()
        {
            _mockRepository.Setup(r => r.GetByUserIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync((CheckingAccount)null);

            var exception = await Assert.ThrowsAsync<CheckingAccountNotFoundException>(
                async () => await _service.GetByUserIdAsync(Guid.NewGuid()));

            Assert.Equal("Checking account not found for the specified user ID. Please check the user ID", exception.Message);
        }

        [Fact]
        public async Task GetByAccountNumberAsync_ShouldReturnAccount_WhenFound()
        {
            var expectedAccount = new CheckingAccount(_testUserId, _mockAccountNumber);
            _mockRepository.Setup(r => r.GetByAccountNumberAsync(_mockAccountNumber))
                           .ReturnsAsync(expectedAccount);

            var result = await _service.GetByAccountNumberAsync(_mockAccountNumber);

            Assert.NotNull(result);
            Assert.Equal(expectedAccount.AccountNumber, result.AccountNumber);
        }

        [Fact]
        public async Task GetByAccountNumberAsync_ShouldReturnNull_WhenNotFound()
        {
            _mockRepository.Setup(r => r.GetByAccountNumberAsync(It.IsAny<string>()))
                           .ReturnsAsync((CheckingAccount)null);

            var result = await _service.GetByAccountNumberAsync("NonExistentAccount");

            Assert.Null(result);
        }

        [Fact]
        public async Task Deposit_ShouldIncreaseAccountBalanceAndSave()
        {
            var initialBalance = 100m;
            var depositAmount = 50m;
            var account = new CheckingAccount(_testUserId, _mockAccountNumber);
            account.Deposit(initialBalance);

            _mockRepository.Setup(r => r.GetByAccountNumberAsync(_mockAccountNumber))
                           .ReturnsAsync(account);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<CheckingAccount>()))
                           .Returns(Task.CompletedTask);

            var updatedAccount = await _service.Deposit(_mockAccountNumber, depositAmount);

            Assert.Equal(initialBalance + depositAmount, updatedAccount.Balance);
            _mockRepository.Verify(r => r.UpdateAsync(account), Times.Once);
            _mockRepository.Verify(r => r.GetByAccountNumberAsync(_mockAccountNumber), Times.Once);
        }

        [Fact]
        public async Task Deposit_ShouldThrowException_WhenAccountNotFoundForDeposit()
        {
            _mockRepository.Setup(r => r.GetByAccountNumberAsync(It.IsAny<string>()))
                           .ReturnsAsync((CheckingAccount)null);

            var exception = await Assert.ThrowsAsync<CheckingAccountNotFoundException>(
                async () => await _service.Deposit(_mockAccountNumber, 100)); 

            Assert.Equal($"Checking account not found for account number: {_mockAccountNumber}.", exception.Message);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<CheckingAccount>()), Times.Never);
        }

        [Fact]
        public async Task WithDraw_ShouldDecreaseAccountBalanceAndSave()
        {
            var initialBalance = 200m;
            var withdrawAmount = 50m;
            var account = new CheckingAccount(_testUserId, _mockAccountNumber);
            account.Deposit(initialBalance); 

            _mockRepository.Setup(r => r.GetByAccountNumberAsync(_mockAccountNumber))
                           .ReturnsAsync(account);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<CheckingAccount>()))
                           .Returns(Task.CompletedTask);

            var updatedAccount = await _service.WithDraw(_mockAccountNumber, withdrawAmount);

            Assert.Equal(initialBalance - withdrawAmount, updatedAccount.Balance);
            _mockRepository.Verify(r => r.UpdateAsync(account), Times.Once);
            _mockRepository.Verify(r => r.GetByAccountNumberAsync(_mockAccountNumber), Times.Once); 
        }

        [Fact]
        public async Task WithDraw_ShouldPropagateInsufficientFundsException()
        {
            var account = new CheckingAccount(_testUserId, _mockAccountNumber); 

            _mockRepository.Setup(r => r.GetByAccountNumberAsync(_mockAccountNumber))
                           .ReturnsAsync(account);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _service.WithDraw(_mockAccountNumber, 100)); 

            Assert.Equal("Insufficient funds", exception.Message);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<CheckingAccount>()), Times.Never);
        }

        [Fact]
        public async Task WithDraw_ShouldThrowException_WhenAccountNotFoundForWithdraw() 
        {

            _mockRepository.Setup(r => r.GetByAccountNumberAsync(It.IsAny<string>()))
                           .ReturnsAsync((CheckingAccount)null);

            var exception = await Assert.ThrowsAsync<CheckingAccountNotFoundException>(
                async () => await _service.WithDraw(_mockAccountNumber, 100)); 

            Assert.Equal($"Checking account not found for account number: {_mockAccountNumber}.", exception.Message);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<CheckingAccount>()), Times.Never);
        }
    }
}