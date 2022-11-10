using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations
{
    public class InviteEntityTypeConfiguration : IEntityTypeConfiguration<Invite>
    {
        public void Configure(EntityTypeBuilder<Invite> builder)
        {
            builder
                .HasKey(b => b.ID);

            builder
            .Property(b => b.ExternalUserAccountId)
            .IsRequired();

            builder
            .Property(b => b.FullName)
            .IsRequired();

            builder
            .Property(b => b.AccountName)
            .IsRequired();

            builder
            .Property(b => b.EmailAddress)
            .IsRequired();

            builder
            .Property(b => b.SentOn)
            .IsRequired();

            builder
            .Property(b => b.ExpiresOn)
            .IsRequired();
        }
    }
}
