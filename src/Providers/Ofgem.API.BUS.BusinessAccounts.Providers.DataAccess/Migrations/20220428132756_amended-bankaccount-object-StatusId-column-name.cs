using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Migrations
{
    public partial class amendedbankaccountobjectStatusIdcolumnname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_BankAccountStatuses_BankAccountStatusID",
                table: "BankAccounts");

            migrationBuilder.RenameColumn(
                name: "BankAccountStatusID",
                table: "BankAccounts",
                newName: "StatusID");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccounts_BankAccountStatusID",
                table: "BankAccounts",
                newName: "IX_BankAccounts_StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_BankAccountStatuses_StatusID",
                table: "BankAccounts",
                column: "StatusID",
                principalTable: "BankAccountStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_BankAccountStatuses_StatusID",
                table: "BankAccounts");

            migrationBuilder.RenameColumn(
                name: "StatusID",
                table: "BankAccounts",
                newName: "BankAccountStatusID");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccounts_StatusID",
                table: "BankAccounts",
                newName: "IX_BankAccounts_BankAccountStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_BankAccountStatuses_BankAccountStatusID",
                table: "BankAccounts",
                column: "BankAccountStatusID",
                principalTable: "BankAccountStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
