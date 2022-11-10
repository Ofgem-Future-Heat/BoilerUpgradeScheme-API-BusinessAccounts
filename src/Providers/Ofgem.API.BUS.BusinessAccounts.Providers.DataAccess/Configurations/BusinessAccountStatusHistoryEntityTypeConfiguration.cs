using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations;

public class BusinessAccountStatusHistoryEntityTypeConfiguration : IEntityTypeConfiguration<BusinessAccountStatusHistory>
{
    public void Configure(EntityTypeBuilder<BusinessAccountStatusHistory> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.BusinessAccountID)
            .IsRequired();

        builder
            .Property(b => b.SubStatusId)
            .IsRequired();

        builder
            .Property(b => b.StartDateTime)
            .IsRequired();

        builder
            .Property(b => b.EndDateTime)
            .IsRequired(false);
    }
}
