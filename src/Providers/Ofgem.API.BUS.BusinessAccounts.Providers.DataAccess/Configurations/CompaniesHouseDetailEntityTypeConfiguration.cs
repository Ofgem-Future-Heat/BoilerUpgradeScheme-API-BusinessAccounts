using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations;

/// <summary>
/// Configuration for the companies house details type
/// </summary>
public class CompaniesHouseDetailEntityTypeConfiguration: IEntityTypeConfiguration<CompaniesHouseDetail>
{
    public void Configure(EntityTypeBuilder<CompaniesHouseDetail> builder)
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
            .Property(b => b.CompanyName)
            .IsUnicode(false)
            .HasMaxLength(100)
            .IsRequired(false);

        builder
            .Property(b => b.CompanyNumber)
            .IsUnicode(false)
            .HasMaxLength(8)
            .IsRequired(false);

        builder
            .Property(b => b.CompanyStatus)
            .IsUnicode(false)
            .HasMaxLength(100)
            .IsRequired(false);

        builder
            .Property(b => b.TradingName)
            .IsUnicode(false)
            .HasMaxLength(100)
            .IsRequired(false);

        builder
            .Property(b => b.RegisteredOffice)
            .IsUnicode(false)
            .HasMaxLength(100)
            .IsRequired(false);

        builder
            .Property(b => b.CompanyRegistrationNumber)
            .IsUnicode(false)
            .HasMaxLength(100)
            .IsRequired(false);

        builder
            .Property(b => b.ParentCompanyName)
            .IsUnicode(false)
            .HasMaxLength(100)
            .IsRequired(false);

        builder
            .Property(b => b.ParentCompanyNumber)
            .IsUnicode(false)
            .HasMaxLength(100)
            .IsRequired(false);

        builder
            .Property(b => b.RegisteredAddressId)
            .IsRequired(false);

        builder
            .Property(b => b.TelephoneNumber)
            .IsUnicode(false)
            .HasMaxLength(15)
            .IsRequired(false);

        builder
            .Property(b => b.EmailAddress)
            .IsUnicode(false)
            .HasMaxLength(100)
            .IsRequired(false);
    }
}
