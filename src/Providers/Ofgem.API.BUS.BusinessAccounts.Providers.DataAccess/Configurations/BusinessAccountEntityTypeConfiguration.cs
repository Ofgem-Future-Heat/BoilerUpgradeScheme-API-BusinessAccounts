using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations;

/// <summary>
/// Configuration for BusinessAccount Entity.
/// </summary>
public class BusinessAccountEntityTypeConfiguration : IEntityTypeConfiguration<BusinessAccount>
{
    public void Configure(EntityTypeBuilder<BusinessAccount> builder)
    {
        builder
            .HasKey(b => b.Id);

        builder
            .Property(b => b.BusinessAccountNumber)
            .IsUnicode(false)
            .HasMaxLength(10)
            .IsRequired();

        builder
            .Property(b => b.AccountSetupRequestDate)
            .IsRequired();

        builder
            .Property(b => b.ActiveDate)
            .IsRequired(false);

        builder
            .Property(b => b.BusinessName)
            .IsUnicode(false)
            .HasMaxLength(100)
            .IsRequired(false);

        builder
            .Property(b => b.TradingName)
            .IsUnicode(false)
            .HasMaxLength(100)
            .IsRequired(false);

        builder
            .Property(b => b.SubStatusId)
            .IsRequired();

        builder
            .Property(b => b.CompanyTypeId)
            .IsRequired(false);

        builder
            .Property(b => b.MCSCertificationNumber)
            .IsUnicode(false)
            .HasMaxLength(15)
            .IsRequired();

        builder
            .Property(b => b.MCSCertificationBody)
            .IsUnicode(false)
            .HasMaxLength(10)
            .IsRequired(false);

        builder
            .Property(b => b.MCSMembershipNumber)
            .IsUnicode(false)
            .HasMaxLength(10)
            .IsRequired(false);

        builder
            .Property(b => b.MCSConsumerCode)
            .IsUnicode(false)
            .HasMaxLength(4)
            .IsFixedLength()
            .IsRequired(false);

        builder
            .Property(b => b.MCSCompanyType)
            .IsUnicode(false)
            .HasMaxLength(25)
            .IsRequired(false);

        builder
            .Property(b => b.MCSId)
            .IsUnicode(false)
            .HasMaxLength(10)
            .IsRequired(false);

        builder
            .Property(b => b.MCSContactDetailsID)
            .IsRequired(false);

        builder
            .Property(b => b.MCSAddressID)
            .IsRequired(false);

        builder
            .Property(b => b.CoHoID)
            .IsRequired(false);

        builder
            .Property(b => b.IsUnderInvestigation)
            .IsRequired();

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
            .Property(b => b.QCRecommendation)
            .IsRequired(false);

        builder
            .Property(b => b.DARecommendation)
            .IsRequired(false);
    }
}
