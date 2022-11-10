﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess;

#nullable disable

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Migrations
{
    [DbContext(typeof(BusinessAccountsDbContext))]
    [Migration("20220428154946_extended-businessaccount-status-and-substatus")]
    partial class extendedbusinessaccountstatusandsubstatus
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AddressLine1")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("AddressLine2")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("AddressLine3")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("AddressLine4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("BusinessAccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("County")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Postcode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("UPRN")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessAccountId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BankAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<Guid>("BusinessAccountID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SortCode")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<Guid?>("StatusID")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SunAccountNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessAccountID");

                    b.HasIndex("StatusID");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BankAccountStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("BankAccountStatuses");
                });

            modelBuilder.Entity("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BusinessAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AccountSetupRequestDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ActiveDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("BusinessAccountNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("BusinessName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("CoHoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CompanyTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsUnderInvestigation")
                        .HasColumnType("bit");

                    b.Property<Guid?>("MCSAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MCSCertificationBody")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("MCSCertificationNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("MCSCompanyType")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("MCSConsumerCode")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<Guid?>("MCSContactDetailsID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MCSId")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("MCSMembershipNumber")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<Guid?>("SubStatusId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TradingName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyTypeId");

                    b.HasIndex("SubStatusId");

                    b.ToTable("BusinessAccounts");
                });

            modelBuilder.Entity("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.CompaniesHouseDetails", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyRegistrationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentCompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentCompanyNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RegisteredAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RegisteredOffice")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelephoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TradingName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CompaniesHouseDetails");
                });

            modelBuilder.Entity("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.CompanyType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("CompanyTypes");
                });

            modelBuilder.Entity("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.ExternalUserAccount", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AADB2CId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BusinessAccountId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CoHoRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FullName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("HomeAddress")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("HomeAddressUPRN")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<bool?>("PrimaryContact")
                        .IsRequired()
                        .HasColumnType("bit");

                    b.Property<Guid?>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("StandardUser")
                        .IsRequired()
                        .HasColumnType("bit");

                    b.Property<bool?>("SuperUser")
                        .IsRequired()
                        .HasColumnType("bit");

                    b.Property<string>("TelephoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("WorkEmailAddress")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ID");

                    b.ToTable("ExternalUsers");
                });

            modelBuilder.Entity("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.GlobalSetting", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid>("GeneratedByID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("NextBusinessAccountReferenceNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NextBusinessAccountReferenceNumber"), 1000000L, 1);

                    b.HasKey("ID");

                    b.ToTable("GlobalSettings");
                });

            modelBuilder.Entity("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.Status", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.SubStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.Property<Guid>("StatusId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.ToTable("SubStatuses");
                });

            modelBuilder.Entity("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.Address", b =>
                {
                    b.HasOne("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BusinessAccount", null)
                        .WithMany("BusinessAddresses")
                        .HasForeignKey("BusinessAccountId");
                });

            modelBuilder.Entity("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BankAccount", b =>
                {
                    b.HasOne("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BusinessAccount", null)
                        .WithMany("BankAccounts")
                        .HasForeignKey("BusinessAccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BankAccountStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BusinessAccount", b =>
                {
                    b.HasOne("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.CompanyType", "CompanyType")
                        .WithMany()
                        .HasForeignKey("CompanyTypeId");

                    b.HasOne("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.SubStatus", "SubStatus")
                        .WithMany()
                        .HasForeignKey("SubStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CompanyType");

                    b.Navigation("SubStatus");
                });

            modelBuilder.Entity("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.SubStatus", b =>
                {
                    b.HasOne("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.Status", "Status")
                        .WithMany("SubStatus")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.BusinessAccount", b =>
                {
                    b.Navigation("BankAccounts");

                    b.Navigation("BusinessAddresses");
                });

            modelBuilder.Entity("Ofgem.API.BUS.BusinessAccounts.Domain.Entities.Status", b =>
                {
                    b.Navigation("SubStatus");
                });
#pragma warning restore 612, 618
        }
    }
}
