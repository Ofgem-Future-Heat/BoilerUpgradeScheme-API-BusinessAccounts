using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Migrations
{
    public partial class renamedbankaccountstatustable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_BankAccountsStatuses_BankAccountStatusID",
                table: "BankAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankAccountsStatuses",
                table: "BankAccountsStatuses");

            migrationBuilder.RenameTable(
                name: "BankAccountsStatuses",
                newName: "BankAccountStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankAccountStatuses",
                table: "BankAccountStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_BankAccountStatuses_BankAccountStatusID",
                table: "BankAccounts",
                column: "BankAccountStatusID",
                principalTable: "BankAccountStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_BankAccountStatuses_BankAccountStatusID",
                table: "BankAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankAccountStatuses",
                table: "BankAccountStatuses");

            migrationBuilder.RenameTable(
                name: "BankAccountStatuses",
                newName: "BankAccountsStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankAccountsStatuses",
                table: "BankAccountsStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_BankAccountsStatuses_BankAccountStatusID",
                table: "BankAccounts",
                column: "BankAccountStatusID",
                principalTable: "BankAccountsStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
