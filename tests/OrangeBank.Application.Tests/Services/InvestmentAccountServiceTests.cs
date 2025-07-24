using System;
using System.Threading.Tasks;
using Moq;
using OrangeBank.Application.Services;
using OrangeBank.Core.Domain.Entities;
using OrangeBank.Core.Domain.Interfaces;
using Xunit;

namespace OrangeBank.Application.Tests.Services
{
    public class InvestmentAccountServiceTests
    {
        private readonly Mock<IInvestmentAccountRepository> _repositoryMock;
        private readonly InvestmentAccountService _service;

        public InvestmentAccountServiceTests()
        {
            _repositoryMock = new Mock<IInvestmentAccountRepository>();
            _service = new InvestmentAccountService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetByAccountNumberAsync_ShouldReturnAccount_WhenAccountExists()
        {
            // Arrange
            var accountNumber = "1234567890123456";
            var expectedAccount = new InvestmentAccount(Guid.NewGuid(), accountNumber);
            _repositoryMock.Setup(r => r.GetByAccountNumberAsync(accountNumber))
                .ReturnsAsync(expectedAccount);

            // Act
            var result = await _service.GetByAccountNumberAsync(accountNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAccount, result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAccount_WhenAccountExists()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var expectedAccount = new InvestmentAccount(accountId, "1234567890123456");
            _repositoryMock.Setup(r => r.GetByIdAsync(accountId))
                .ReturnsAsync(expectedAccount);

            // Act
            var result = await _service.GetByIdAsync(accountId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAccount, result);
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnAccount_WhenAccountExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedAccount = new InvestmentAccount(userId, "1234567890123456");
            _repositoryMock.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(expectedAccount);

            // Act
            var result = await _service.GetByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAccount, result);
        }

        [Fact]
        public async Task RegisterAsync_ShouldCreateAccount_WhenAccountNumberIsUnique()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var generatedAccountNumber = "1234567890123456";
            _repositoryMock.Setup(r => r.AccountNumberExists(generatedAccountNumber))
                .ReturnsAsync(true);
            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<InvestmentAccount>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.RegisterAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.NotEqual(generatedAccountNumber, result.AccountNumber);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<InvestmentAccount>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenMaxRetriesExceeded()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.AccountNumberExists(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _service.RegisterAsync(userId));
        }
    }
}
