using System;
using OrangeJuiceBank.Core.Domain.Enums;

public class Asset
{
	public Guid Id { get; private set; }
	public string Code { get; private set; }
    public string Name { get; private set; }
	public AssetType Type { get; private set; }
	public decimal Price { get; private set; }
	public decimal DailyVariation { get; private set; }
	public string Description { get; private set; }
	public AssetRisk risk { get; private set; }
	public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }


	public Asset(string code, string name, AssetType type, decimal price, decimal dailyVariation, string description, AssetRisk risk)
    {
        Id = Guid.NewGuid();
        this.code = code;
        Name = name;
        Type = type;
        Price = price;
        DailyVariation = dailyVariation;
        Description = description;
        this.risk = risk;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

}


