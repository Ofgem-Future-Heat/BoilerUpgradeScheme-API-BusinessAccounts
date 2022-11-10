using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubStatuses_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TradingName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BusinessAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MCSCertificationNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MCSCertificationBody = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MCSMembershipNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MCSConsumerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MCSCompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MCSCompanyType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MCSId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MCSAddressId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessAccounts_CompanyTypes_CompanyTypeId",
                        column: x => x.CompanyTypeId,
                        principalTable: "CompanyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessAccounts_SubStatuses_SubStatusId",
                        column: x => x.SubStatusId,
                        principalTable: "SubStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UPRN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddressLine3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    County = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    BusinessAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_BusinessAccounts_BusinessAccountId",
                        column: x => x.BusinessAccountId,
                        principalTable: "BusinessAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SortCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BusinessAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_BusinessAccounts_BusinessAccountId",
                        column: x => x.BusinessAccountId,
                        principalTable: "BusinessAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_BusinessAccountId",
                table: "Addresses",
                column: "BusinessAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_BusinessAccountId",
                table: "BankAccounts",
                column: "BusinessAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessAccounts_CompanyTypeId",
                table: "BusinessAccounts",
                column: "CompanyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessAccounts_SubStatusId",
                table: "BusinessAccounts",
                column: "SubStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatuses_StatusId",
                table: "SubStatuses",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "BusinessAccounts");

            migrationBuilder.DropTable(
                name: "CompanyTypes");

            migrationBuilder.DropTable(
                name: "SubStatuses");

            migrationBuilder.DropTable(
                name: "Statuses");
        }
    }
}
