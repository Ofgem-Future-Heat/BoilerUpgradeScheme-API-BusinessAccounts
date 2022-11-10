using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Migrations
{
    public partial class bankaccountchangesandaddedbankaccountstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "BankAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "SunAccountNumber",
                table: "BankAccounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SortCode",
                table: "BankAccounts",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8,
                oldNullable: true);

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

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "BankAccounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BankAccountStatusID",
                table: "BankAccounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "BankAccountsStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountsStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_BankAccountStatusID",
                table: "BankAccounts",
                column: "BankAccountStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_BankAccountsStatuses_BankAccountStatusID",
                table: "BankAccounts",
                column: "BankAccountStatusID",
                principalTable: "BankAccountsStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_BankAccountsStatuses_BankAccountStatusID",
                table: "BankAccounts");

            migrationBuilder.DropTable(
                name: "BankAccountsStatuses");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_BankAccountStatusID",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "BankAccountStatusID",
                table: "BankAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "SunAccountNumber",
                table: "BankAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SortCode",
                table: "BankAccounts",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "BankAccounts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "BankAccounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<Guid>(
                name: "StatusID",
                table: "BankAccounts",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
