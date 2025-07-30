using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrangeJuiceBank.Core.Domain.Enums;

namespace OrangeBank.Core.Domain.Entities;

public class PortfolioItem: IEntity
{
    public Guid Id { get; private set; }
    public Guid AssetId { get; private set; }
    public Asset Asset { get; private set; }
    public int Quantity { get; private set; }
    public decimal AveragePrice { get; private set; }
    public ICollection<Order> Orders { get; private set; } = new List<Order>();
    public DateTime CreatedAt { get; private set; }

    protected PortfolioItem() { }

    public PortfolioItem(Guid assetId, int quantity, decimal unitPrice)
    {
        Id = Guid.NewGuid();
        AssetId = assetId;
        Quantity = quantity;
        AveragePrice = unitPrice;
    }

    public void AddOrder(Order order)
    {
        Orders.Add(order);

        if (order.Type == OrderType.Buy)
        {
            Quantity += order.Quantity;
            AveragePrice = ((AveragePrice * (Quantity - order.Quantity)) +
                            (order.UnitPrice * order.Quantity)) / Quantity;
        }
        else if (order.Type == OrderType.Sell)
        {
            Quantity -= order.Quantity;
        }
    }
}

