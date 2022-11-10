using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations;

/// <summary>
/// Configuration for CompanyType Entity.
/// </summary>
public class CompanyTypeEntityTypeConfiguration : IEntityTypeConfiguration<CompanyType>
{
    public void Configure(EntityTypeBuilder<CompanyType> builder)
    {
        builder
            .HasKey(b => b.Id);

        builder
            .Property(b => b.Description)
            .IsUnicode(false)
            .HasMaxLength(255)
            .IsRequired();
    }
}
