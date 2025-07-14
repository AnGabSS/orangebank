using Microsoft.EntityFrameworkCore;
using OrangeBank.Core.Domain.Entities;

namespace OrangeBank.Infrastructure.Data;

public class OrangeBankContext : DbContext
{
    public OrangeBankContext(DbContextOptions<OrangeBankContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.Property(u => u.CPF).IsRequired().HasMaxLength(11);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.CPF).IsUnique();
            entity.Property(u => u.Name).IsRequired().HasMaxLength(255);
            entity.Property(u => u.Password).IsRequired().HasMaxLength(255);
            entity.Property(u => u.CreatedAt).HasDefaultValue(DateTime.Now);
        });
    }
}