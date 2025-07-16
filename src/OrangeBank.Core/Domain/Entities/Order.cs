using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrangeBank.Core.Domain.Exceptions;
using OrangeJuiceBank.Core.Domain.Enums;

namespace OrangeBank.Core.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid PortfolioId { get; private set; }
    public OrderType Type { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice => Quantity * UnitPrice;
    public decimal Fees { get; private set; }
    public decimal Tax { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public OrderStatus Status { get; private set; }
    public Guid AssetId { get; private set; } 

    public Order() { }

    public Order(Guid portfolioId, OrderType type, int quantity, decimal unitPrice, decimal fees, decimal tax, Guid assetId)
    {
        Id = Guid.NewGuid();
        PortfolioId = portfolioId;
        Type = type;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Fees = fees;
        Tax = tax;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Status = OrderStatus.Pending;
        AssetId = assetId;
    }

}


