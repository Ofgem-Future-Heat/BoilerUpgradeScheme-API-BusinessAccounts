using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Migrations
{
    public partial class ExternalUserAccount_AddTermsLastUpdatedVersion_Time : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TermsLastAcceptedDate",
                table: "ExternalUserAccounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TermsLastAcceptedVersion",
                table: "ExternalUserAccounts",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TermsLastAcceptedDate",
                table: "ExternalUserAccounts");

            migrationBuilder.DropColumn(
                name: "TermsLastAcceptedVersion",
                table: "ExternalUserAccounts");
        }
    }
}
