using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeBank.Core.Domain.Entities
{
    public class CheckingAccount : Account
    {
        private const decimal DailyWithdrawalLimit = 5000m;
        public decimal DailyWithdrawn { get; private set; }
        public DateTime LastWithdrawalDate { get; private set; }

        public CheckingAccount(Guid userId, string accountNumber)
            : base(userId, accountNumber)
        {
            DailyWithdrawn = 0;
            LastWithdrawalDate = DateTime.UtcNow.Date;
        }

        public override void Withdraw(decimal amount)
        {
            if (!CanWithdraw(amount))
                throw new InvalidOperationException("Withdrawal limit exceeded");

            base.Withdraw(amount);
            UpdateWithdrawalTracking(amount);
        }

        private void UpdateWithdrawalTracking(decimal amount)
        {
            var today = DateTime.UtcNow.Date;

            if (LastWithdrawalDate != today)
            {
                DailyWithdrawn = 0;
                LastWithdrawalDate = today;
            }

            DailyWithdrawn += amount;
        }

        protected override bool CanWithdraw(decimal amount)
        {
            var today = DateTime.UtcNow.Date;

            if (LastWithdrawalDate != today)
                return amount <= DailyWithdrawalLimit;

            return (DailyWithdrawn + amount) <= DailyWithdrawalLimit;
        }
    }
}
