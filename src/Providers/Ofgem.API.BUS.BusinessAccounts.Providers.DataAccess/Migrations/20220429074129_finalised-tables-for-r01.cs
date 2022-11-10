using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Migrations
{
    public partial class finalisedtablesforr01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MCSAddressId",
                table: "BusinessAccounts",
                newName: "MCSAddressID");

            migrationBuilder.RenameColumn(
                name: "CoHoId",
                table: "BusinessAccounts",
                newName: "CoHoID");

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "ExternalUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AuthorisedRepresentative",
                table: "ExternalUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                table: "ExternalUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeAddressPostcode",
                table: "ExternalUsers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RegisteredAddressId",
                table: "CompaniesHouseDetails",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessAccountID",
                table: "CompaniesHouseDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "BusinessAccounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "BusinessAccounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DARecommendation",
                table: "BusinessAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastUpdatedBy",
                table: "BusinessAccounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedDate",
                table: "BusinessAccounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QCRecommendation",
                table: "BusinessAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UPRN",
                table: "Addresses",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "Postcode",
                table: "Addresses",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompaniesHouseDetails_BusinessAccountID",
                table: "CompaniesHouseDetails",
                column: "BusinessAccountID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompaniesHouseDetails_BusinessAccounts_BusinessAccountID",
                table: "CompaniesHouseDetails",
                column: "BusinessAccountID",
                principalTable: "BusinessAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompaniesHouseDetails_BusinessAccounts_BusinessAccountID",
                table: "CompaniesHouseDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompaniesHouseDetails_BusinessAccountID",
                table: "CompaniesHouseDetails");

            migrationBuilder.DropColumn(
                name: "AuthorisedRepresentative",
                table: "ExternalUsers");

            migrationBuilder.DropColumn(
                name: "DOB",
                table: "ExternalUsers");

            migrationBuilder.DropColumn(
                name: "HomeAddressPostcode",
                table: "ExternalUsers");

            migrationBuilder.DropColumn(
                name: "BusinessAccountID",
                table: "CompaniesHouseDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BusinessAccounts");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "BusinessAccounts");

            migrationBuilder.DropColumn(
                name: "DARecommendation",
                table: "BusinessAccounts");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "BusinessAccounts");

            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                table: "BusinessAccounts");

            migrationBuilder.DropColumn(
                name: "QCRecommendation",
                table: "BusinessAccounts");

            migrationBuilder.RenameColumn(
                name: "MCSAddressID",
                table: "BusinessAccounts",
                newName: "MCSAddressId");

            migrationBuilder.RenameColumn(
                name: "CoHoID",
                table: "BusinessAccounts",
                newName: "CoHoId");

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "ExternalUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<Guid>(
                name: "RegisteredAddressId",
                table: "CompaniesHouseDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UPRN",
                table: "Addresses",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Postcode",
                table: "Addresses",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }
    }
}
