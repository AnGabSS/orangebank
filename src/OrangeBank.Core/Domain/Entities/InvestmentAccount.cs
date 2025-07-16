using OrangeJuiceBank.Core.Domain.Enums;

namespace OrangeBank.Core.Domain.Entities
{
    public class InvestmentAccount : Account
    {
        public decimal InvestedBalance { get; private set; }
        public ICollection<PortfolioItem> Portfolio { get; private set; } = new List<PortfolioItem>();

        public InvestmentAccount(Guid userId, string accountNumber)
            : base(userId, accountNumber)
        {
            InvestedBalance = 0;
        }

        public override void Withdraw(decimal amount)
        {
            if (!CanWithdraw(amount))
                throw new InvalidOperationException("Cannot withdraw from investment account with pending orders");

            base.Withdraw(amount);
        }

        public void Invest(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Investment amount must be positive");

            if (Balance < amount)
                throw new InvalidOperationException("Insufficient funds");

            Balance -= amount;
            InvestedBalance += amount;
        }

        public void ReceiveReturn(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Return amount must be positive");

            Balance += amount;
            InvestedBalance -= amount;
        }

        protected override bool CanWithdraw(decimal amount)
        {
            bool hasPendingOrders = Portfolio.Any(p => p.Orders.Any(o => o.Status == OrderStatus.Pending));

            return !hasPendingOrders && Balance >= amount;
        }
    }
}
