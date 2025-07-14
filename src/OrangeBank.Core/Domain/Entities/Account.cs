using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeBank.Core.Domain.Entities
{
    public abstract class Account
    {
        public Guid Id { get; protected set; }
        public string AccountNumber { get; protected set; }
        public decimal Balance { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public bool IsActive { get; protected set; }
        public Guid UserId { get; protected set; }

        protected Account(Guid userId, string accountNumber)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            AccountNumber = accountNumber;
            Balance = 0;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public virtual void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Deposit amount must be positive");

            Balance += amount;
        }

        public virtual void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Withdrawal amount must be positive");

            if (Balance < amount)
                throw new InvalidOperationException("Insufficient funds");

            Balance -= amount;
        }

        protected abstract bool CanWithdraw(decimal amount);
    }
}
