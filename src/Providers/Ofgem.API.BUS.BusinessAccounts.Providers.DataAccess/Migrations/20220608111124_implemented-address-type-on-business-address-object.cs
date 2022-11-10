using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Migrations
{
    public partial class implementedaddresstypeonbusinessaddressobject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AddressTypes",
                columns: new[] { "Id", "Description", "Code", "DisplayName", "SortOrder" },
                values: new object[] {
                    "2B7C0FBE-81FB-48AF-B3D4-99B6941C3CB02", 
                    "Represents the address type related to the business address of a business/installer account.", 
                    "BIZ", 
                    "Business Address", 
                    10
                });

            migrationBuilder.InsertData(
                table: "AddressTypes",
                columns: new[] { "Id", "Description", "Code", "DisplayName", "SortOrder" },
                values: new object[] {
                    "454ACCB1-3D67-4FCC-A92E-DCB07384CA14",
                    "Represents the address type related to the trading address of a business/installer account.", 
                    "TRADE",
                    "Trading Address",
                    20
                });

            migrationBuilder.AddColumn<Guid>(
                name: "AddressTypeId",
                table: "BusinessAddresses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("2B7C0FBE-81FB-48AF-B3D4-99B6941C3CB0"));

            migrationBuilder.CreateIndex(
                name: "IX_BusinessAddresses_AddressTypeId",
                table: "BusinessAddresses",
                column: "AddressTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessAddresses_AddressTypes_AddressTypeId",
                table: "BusinessAddresses",
                column: "AddressTypeId",
                principalTable: "AddressTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessAddresses_AddressTypes_AddressTypeId",
                table: "BusinessAddresses");

            migrationBuilder.DropIndex(
                name: "IX_BusinessAddresses_AddressTypeId",
                table: "BusinessAddresses");

            migrationBuilder.DropColumn(
                name: "AddressTypeId",
                table: "BusinessAddresses");
        }
    }
}
