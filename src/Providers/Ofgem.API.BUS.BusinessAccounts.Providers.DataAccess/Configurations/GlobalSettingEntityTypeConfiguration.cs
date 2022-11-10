using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations;

public class GlobalSettingEntityTypeConfiguration : IEntityTypeConfiguration<GlobalSetting>
{
    public void Configure(EntityTypeBuilder<GlobalSetting> builder)
    {
        builder.HasKey(b => b.ID);
        builder.Property(b => b.NextBusinessAccountReferenceNumber).UseIdentityColumn(1000000, 1);
        builder.Property(b => b.GeneratedByID).IsRequired();
    }
}
