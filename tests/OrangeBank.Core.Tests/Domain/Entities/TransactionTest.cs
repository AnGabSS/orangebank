using Xunit;
using FluentAssertions;
using OrangeBank.Core.Domain.Entities;
using OrangeBank.Core.Domain.Enums;
using System;

namespace OrangeBank.Core.Domain.Tests.Entities
{
    public class TransactionTests
    {
        private const string Origin = "0001-1/12345-6";
        private const string Destiny = "0001-1/65432-1";

        [Fact]
        public void Transaction_Constructor_InitializesCorrectly()
        {
            var amount = 150.75m;
            var type = TransactionType.EXTERNAL;

            var t = new Transaction(Origin, Destiny, amount, type);

            t.Id.Should().NotBeEmpty();
            t.OriginAccount.Should().Be(Origin);
            t.DestinyAccount.Should().Be(Destiny);
            t.Amount.Should().Be(amount);
            t.type.Should().Be(type);
            t.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void Transaction_Constructor_SetsCreatedAtAsUtc()
        {
            var t = new Transaction(Origin, Destiny, 10m, TransactionType.INTERNAL);
            t.CreatedAt.Kind.Should().Be(DateTimeKind.Utc);
        }

        [Fact]
        public void Transaction_Ids_ShouldBeUnique()
        {
            var a = new Transaction(Origin, Destiny, 10m, TransactionType.INTERNAL);
            var b = new Transaction(Origin, Destiny, 10m, TransactionType.INTERNAL);
            a.Id.Should().NotBe(b.Id);
        }

        [Fact]
        public void Transaction_OriginAccount_Null_ShouldThrowArgumentNullException()
        {
            Action act = () => new Transaction(null!, Destiny, 10m, TransactionType.INTERNAL);
            act.Should().Throw<ArgumentNullException>().WithParameterName("originAccount");
        }

        [Fact]
        public void Transaction_DestinyAccount_Null_ShouldThrowArgumentNullException()
        {
            Action act = () => new Transaction(Origin, null!, 10m, TransactionType.INTERNAL);
            act.Should().Throw<ArgumentNullException>().WithParameterName("destinyAccount");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(-50)]
        public void Transaction_Constructor_AssignsAmount_AsProvided(decimal amount)
        {
            var t = new Transaction(Origin, Destiny, amount, TransactionType.INTERNAL);
            t.Amount.Should().Be(amount);
        }

        [Theory]
        [InlineData(TransactionType.EXTERNAL)]
        [InlineData(TransactionType.INTERNAL)]
        public void Transaction_Constructor_AssignsType_AsProvided(TransactionType type)
        {
            var t = new Transaction(Origin, Destiny, 1m, type);
            t.type.Should().Be(type);
        }
    }
}
