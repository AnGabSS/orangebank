namespace OrangeBank.Core.Domain.Entities;

public class Portfolio
{
    public Guid id { get; set; }
    public string accountId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}