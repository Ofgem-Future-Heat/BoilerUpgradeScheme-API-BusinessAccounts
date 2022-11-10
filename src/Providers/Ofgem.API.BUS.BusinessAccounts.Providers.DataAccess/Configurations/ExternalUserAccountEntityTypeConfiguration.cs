using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations;

/// <summary>
/// Configuration for the External User Account type
/// </summary>
public class ExternalUserAccountEntityTypeConfiguration : IEntityTypeConfiguration<ExternalUserAccount>
{
    public void Configure(EntityTypeBuilder<ExternalUserAccount> builder)
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
            .Property(b => b.BusinessAccountID)
            .IsRequired();

        builder
            .Property(b => b.FullName)
            .IsUnicode(false)
            .HasMaxLength(100)
            .IsRequired(false);

        builder
            .Property(b => b.RoleId)
            .IsRequired(false);

        builder
            .Property(b => b.TelephoneNumber)
            .IsUnicode(false)
            .HasMaxLength(20)
            .IsRequired(false);

        builder
            .Property(b => b.EmailAddress)
            .IsUnicode(false)
            .HasMaxLength(254)
            .IsRequired();

        builder
            .Property(b => b.HomeAddressUPRN)
            .IsUnicode(false)
            .HasMaxLength(12)
            .IsRequired(false);

        builder
            .Property(b => b.HomeAddress)
            .IsUnicode(false)
            .HasMaxLength(255)
            .IsRequired(false);

        builder
            .Property(b => b.HomeAddressPostcode)
            .IsUnicode(false)
            .HasMaxLength(8)
            .IsRequired(false);

        builder
            .Property(b => b.CoHoRoleID)
            .IsRequired(false);

        builder
            .Property(b => b.SuperUser)
            .IsRequired();

        builder
            .Property(b => b.StandardUser)
            .IsRequired();

        builder
            .Property(b => b.DOB)
            .IsRequired(false);

        builder
            .Property(b => b.AuthorisedRepresentative)
            .IsRequired();

        builder
            .Property(b => b.AADB2CId)
            .IsRequired(false);

        builder
            .Property(b => b.FirstName)
            .IsUnicode(false)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(b => b.LastName)
            .IsUnicode(false)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(b => b.TermsLastAcceptedDate)
            .IsRequired(false);

        builder
            .Property(b => b.TermsLastAcceptedVersion)
            .IsRequired(false);
    }
}
