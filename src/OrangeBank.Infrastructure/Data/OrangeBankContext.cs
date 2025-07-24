using Microsoft.EntityFrameworkCore;
using OrangeBank.Core.Domain.Entities;

namespace OrangeBank.Infrastructure.Data;

public class OrangeBankContext : DbContext
{
    public OrangeBankContext(DbContextOptions<OrangeBankContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<CheckingAccount> CheckingAccount { get; set; }
    public DbSet<InvestmentAccount> InvestmentAccount { get; set; }

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
        modelBuilder.Entity<CheckingAccount>(entity =>
        {
            entity.HasKey(ca => ca.Id);
            entity.HasIndex(ca => ca.AccountNumber).IsUnique();
            entity.Property(ca => ca.UserId).IsRequired();
            entity.HasIndex(ca => ca.UserId).IsUnique();
            entity.Property(ca => ca.CreatedAt).HasDefaultValue(DateTime.Now);
            entity.Property(ca => ca.IsActive).HasDefaultValue(true);
        });
        modelBuilder.Entity<InvestmentAccount>(entity =>
        {
            entity.HasKey(ia => ia.Id);
            entity.HasIndex(ia => ia.AccountNumber).IsUnique();
            entity.Property(ia => ia.UserId).IsRequired();
            entity.HasIndex(ia => ia.UserId).IsUnique();
            entity.Property(ia => ia.CreatedAt).HasDefaultValue(DateTime.Now);
            entity.Property(ia => ia.IsActive).HasDefaultValue(true);
        });
    }
}