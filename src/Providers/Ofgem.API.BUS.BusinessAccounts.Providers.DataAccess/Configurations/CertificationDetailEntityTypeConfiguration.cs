using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations
{
    internal class CertificationDetailEntityTypeConfiguration : IEntityTypeConfiguration<CertificationDetail>
    {
        public void Configure(EntityTypeBuilder<CertificationDetail> builder)
        {
            builder
                .HasKey(b => b.Id);

            builder
                .Property(b => b.BusinessAccountID)
                .IsRequired();

            builder
                .Property(b => b.TechTypeCertificationID)
                .IsRequired();

            builder
                .Property(b => b.StartDate)
                .IsRequired();

            builder
                .Property(b => b.ExpiryDate)
                .IsRequired();
        }
    }
}
