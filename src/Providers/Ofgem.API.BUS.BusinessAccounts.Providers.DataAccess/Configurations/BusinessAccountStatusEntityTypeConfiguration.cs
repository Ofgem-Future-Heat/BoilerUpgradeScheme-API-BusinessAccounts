using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations;

/// <summary>
/// Configuration for Status Entity.
/// </summary>
public class BusinessAccountStatusEntityTypeConfiguration : IEntityTypeConfiguration<BusinessAccountStatus>
{
    public void Configure(EntityTypeBuilder<BusinessAccountStatus> builder)
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
            .HasMaxLength(20)
            .IsRequired();

        builder
            .Property(b => b.Description)
            .IsUnicode(false)
            .HasMaxLength(20)
            .IsRequired();

        builder
            .Property(b => b.SortOrder)
            .IsRequired();

        builder
            .HasMany(b => b.BusinessAccountSubStatuses)
            .WithOne(b => b.BusinessAccountStatus);
    }
}
