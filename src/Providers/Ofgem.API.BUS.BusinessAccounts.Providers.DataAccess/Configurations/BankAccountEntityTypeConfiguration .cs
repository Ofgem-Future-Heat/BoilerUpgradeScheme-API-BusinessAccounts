using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations;

/// <summary>
/// Configuration for BankAccount Entity.
/// </summary>
public class BankAccountEntityTypeConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder
            .HasKey(b => b.Id);

        builder
            .Property(b => b.CreatedDate)
            .IsRequired();

        builder
            .Property(b => b.CreatedBy)
            .IsUnicode(false)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(b => b.LastUpdatedDate)
            .IsRequired(false);

        builder
            .Property(b => b.LastUpdatedBy)
            .IsUnicode(false)
            .HasMaxLength(100)
            .IsRequired(false);

        builder
            .Property(b => b.AccountName)
            .IsUnicode(false)
            .HasMaxLength(155)
            .IsRequired();

        builder.
            Property(b => b.SortCode)
            .IsUnicode(false)
            .HasMaxLength(2)
            .IsFixedLength()
            .IsRequired();

        builder
            .Property(b => b.AccountNumber)
            .IsUnicode(false)
            .HasMaxLength(4)
            .IsFixedLength()
            .IsRequired();

        builder
            .Property(b => b.SunAccountNumber)
            .IsUnicode(false)
            .HasMaxLength(10)
            .IsRequired(false);

        builder
            .Property(b => b.BusinessAccountID)
            .IsRequired();

        builder
            .Property(b => b.StatusID)
            .IsRequired();
    }
}
