using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrangeBank.Core.Domain.Enums;

namespace OrangeBank.Core.Domain.Entities
{
    public class Transaction: IEntity
    {

        public Guid Id { get; protected set; }
        public string OriginAccount { get; protected set; }
        public string DestinyAccount { get; protected set; }
        public decimal Amount { get; protected set; }
        public TransactionType type { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        public Transaction(string originAccount, string destinyAccount, decimal amount, TransactionType type)
        {
            Id = Guid.NewGuid();
            OriginAccount = originAccount ?? throw new ArgumentNullException(nameof(originAccount));
            DestinyAccount = destinyAccount ?? throw new ArgumentNullException(nameof(destinyAccount));
            Amount = amount;
            this.type = type;
            CreatedAt = DateTime.UtcNow;
        }

    }
}
