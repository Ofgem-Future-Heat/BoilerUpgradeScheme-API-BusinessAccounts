﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ofgem.API.BUS.BusinessAccounts.Domain.Entities;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Configurations;

/// <summary>
/// Configuration for the SubStatus Entity.
/// </summary>
public class BusinessAccountSubStatusEntityTypeConfiguration : IEntityTypeConfiguration<BusinessAccountSubStatus>
{
    public void Configure(EntityTypeBuilder<BusinessAccountSubStatus> builder)
    {
        builder
            .HasKey(b => b.Id);

        builder
            .Property(b => b.Code)
            .HasConversion<string>()
            .IsUnicode(false)
            .HasMaxLength(32)
            .IsRequired();

        builder
            .Property(b => b.DisplayName)
            .IsUnicode(false)
            .HasMaxLength(20)
            .IsRequired();

        builder
            .Property(b => b.Description)
            .IsUnicode(false)
            .HasMaxLength(20)
            .IsRequired();

        builder
            .Property(b => b.SortOrder)
            .IsRequired();

        builder
            .Property(b => b.BusinessAccountStatusId)
            .IsRequired();
    }
}
