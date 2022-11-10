using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations;

public class BankAccountStatusEntityTypeConfiguration : IEntityTypeConfiguration<BankAccountStatus>
{
    public void Configure(EntityTypeBuilder<BankAccountStatus> builder)
    {
        builder
            .HasKey(b => b.Id);

        builder
            .Property(b => b.Code)
            .HasConversion<string>()
            .IsUnicode(false)
            .HasMaxLength(32)
            .IsRequired();

        builder
            .Property(b => b.DisplayName)
            .IsUnicode(false)
            .HasMaxLength(10)
            .IsRequired();

        builder
            .Property(b => b.Description)
            .IsUnicode(false)
            .HasMaxLength(10)
            .IsRequired();

        builder
            .Property(b => b.SortOrder)
            .IsRequired();
    }
}
