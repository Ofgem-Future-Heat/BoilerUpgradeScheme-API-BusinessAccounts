using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations;

/// <summary>
/// Configuration for CompanyType Entity.
/// </summary>
public class AddressTypeEntityTypeConfiguration : IEntityTypeConfiguration<AddressType>
{
    public void Configure(EntityTypeBuilder<AddressType> builder)
    {
        builder
            .HasKey(b => b.Id);

        builder
            .Property(b => b.Description)
            .IsUnicode(false)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(b => b.Code)
            .HasConversion<string>()
            .IsUnicode(false)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(b => b.DisplayName)
            .IsUnicode(false)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.SortOrder)
            .IsRequired();
    }
}
