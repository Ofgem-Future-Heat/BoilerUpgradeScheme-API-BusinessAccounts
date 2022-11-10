using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Migrations
{
    public partial class ExternalUserandModelUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_BusinessAccounts_BusinessAccountId",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessAccounts_CompanyTypes_CompanyTypeId",
                table: "BusinessAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessAccounts_SubStatuses_SubStatusId",
                table: "BusinessAccounts");

            migrationBuilder.DropColumn(
                name: "MCSCompanyName",
                table: "BusinessAccounts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BankAccounts");

            migrationBuilder.RenameColumn(
                name: "BusinessAccountId",
                table: "BankAccounts",
                newName: "BusinessAccountID");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccounts_BusinessAccountId",
                table: "BankAccounts",
                newName: "IX_BankAccounts_BusinessAccountID");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubStatusId",
                table: "BusinessAccounts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "MCSMembershipNumber",
                table: "BusinessAccounts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "MCSId",
                table: "BusinessAccounts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "MCSConsumerCode",
                table: "BusinessAccounts",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "MCSCompanyType",
                table: "BusinessAccounts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "MCSCertificationBody",
                table: "BusinessAccounts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<Guid>(
                name: "MCSAddressId",
                table: "BusinessAccounts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyTypeId",
                table: "BusinessAccounts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "BusinessAddressId",
                table: "BusinessAccounts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<DateTime>(
                name: "AccountSetupRequestDate",
                table: "BusinessAccounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "BusinessAccountNumber",
                table: "BusinessAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CoHoId",
                table: "BusinessAccounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MCSContactDetailsID",
                table: "BusinessAccounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SortCode",
                table: "BankAccounts",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);

            migrationBuilder.AlterColumn<Guid>(
                name: "BusinessAccountID",
                table: "BankAccounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "BankAccounts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "BankAccounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StatusID",
                table: "BankAccounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SunAccountNumber",
                table: "BankAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine4",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExternalUsers",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TelephoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WorkEmailAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HomeAddressUPRN = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    HomeAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CoHoRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PrimaryContact = table.Column<bool>(type: "bit", nullable: false),
                    SuperUser = table.Column<bool>(type: "bit", nullable: false),
                    StandardUser = table.Column<bool>(type: "bit", nullable: false),
                    AADB2CId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalUsers", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_BusinessAccounts_BusinessAccountID",
                table: "BankAccounts",
                column: "BusinessAccountID",
                principalTable: "BusinessAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessAccounts_CompanyTypes_CompanyTypeId",
                table: "BusinessAccounts",
                column: "CompanyTypeId",
                principalTable: "CompanyTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessAccounts_SubStatuses_SubStatusId",
                table: "BusinessAccounts",
                column: "SubStatusId",
                principalTable: "SubStatuses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_BusinessAccounts_BusinessAccountID",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessAccounts_CompanyTypes_CompanyTypeId",
                table: "BusinessAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessAccounts_SubStatuses_SubStatusId",
                table: "BusinessAccounts");

            migrationBuilder.DropTable(
                name: "ExternalUsers");

            migrationBuilder.DropColumn(
                name: "AccountSetupRequestDate",
                table: "BusinessAccounts");

            migrationBuilder.DropColumn(
                name: "BusinessAccountNumber",
                table: "BusinessAccounts");

            migrationBuilder.DropColumn(
                name: "CoHoId",
                table: "BusinessAccounts");

            migrationBuilder.DropColumn(
                name: "MCSContactDetailsID",
                table: "BusinessAccounts");

            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "SunAccountNumber",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "AddressLine4",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "BusinessAccountID",
                table: "BankAccounts",
                newName: "BusinessAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccounts_BusinessAccountID",
                table: "BankAccounts",
                newName: "IX_BankAccounts_BusinessAccountId");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubStatusId",
                table: "BusinessAccounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MCSMembershipNumber",
                table: "BusinessAccounts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MCSId",
                table: "BusinessAccounts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MCSConsumerCode",
                table: "BusinessAccounts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MCSCompanyType",
                table: "BusinessAccounts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MCSCertificationBody",
                table: "BusinessAccounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MCSAddressId",
                table: "BusinessAccounts",
                type: "uniqueidentifier",
                maxLength: 50,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyTypeId",
                table: "BusinessAccounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BusinessAddressId",
                table: "BusinessAccounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MCSCompanyName",
                table: "BusinessAccounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "SortCode",
                table: "BankAccounts",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BusinessAccountId",
                table: "BankAccounts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "BankAccounts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BankAccounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_BusinessAccounts_BusinessAccountId",
                table: "BankAccounts",
                column: "BusinessAccountId",
                principalTable: "BusinessAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessAccounts_CompanyTypes_CompanyTypeId",
                table: "BusinessAccounts",
                column: "CompanyTypeId",
                principalTable: "CompanyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessAccounts_SubStatuses_SubStatusId",
                table: "BusinessAccounts",
                column: "SubStatusId",
                principalTable: "SubStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
