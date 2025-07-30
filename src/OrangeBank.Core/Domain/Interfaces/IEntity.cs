using System;

public interface IEntity
{
    Guid Id { get; }
    public DateTime CreatedAt { get; }
}
