using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using Ofgem.Lib.BUS.AuditLogging.Configurations;
using Ofgem.Lib.BUS.AuditLogging.Domain.Entities;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations;

public class AuditLogEntityTypeConfiguration : BaseAuditLogEntityTypeConfiguration
{
    public override void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        base.Configure(builder);

        builder.HasOne<BusinessAccount>()
               .WithMany()
               .HasForeignKey("EntityId");
    }
}
