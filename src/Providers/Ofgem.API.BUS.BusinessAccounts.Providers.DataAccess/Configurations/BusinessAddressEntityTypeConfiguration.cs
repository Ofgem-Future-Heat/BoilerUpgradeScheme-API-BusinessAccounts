using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations;

/// <summary>
/// Configuration for Address Entity.
/// </summary>
public class BusinessAddressEntityTypeConfiguration : IEntityTypeConfiguration<BusinessAddress>
{
    public void Configure(EntityTypeBuilder<BusinessAddress> builder)
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
            .Property(b => b.UPRN)
            .IsUnicode(false)
            .HasMaxLength(12)
            .IsRequired(false);

        builder
            .Property(b => b.AddressLine1)
            .IsUnicode(false)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(b => b.AddressLine2)
            .IsUnicode(false)
            .HasMaxLength(255)
            .IsRequired(false);

        builder
            .Property(b => b.AddressLine3)
            .IsUnicode(false)
            .HasMaxLength(255)
            .IsRequired(false);

        builder
            .Property(b => b.AddressLine4)
            .IsUnicode(false)
            .HasMaxLength(255)
            .IsRequired(false);

        builder
            .Property(b => b.County)
            .IsUnicode(false)
            .HasMaxLength(255).IsRequired(false);

        builder
            .Property(b => b.Postcode)
            .IsUnicode(false)
            .HasMaxLength(10)
            .IsRequired();

        builder
            .Property(b => b.AddressTypeId)
            .IsRequired();
    }
}
