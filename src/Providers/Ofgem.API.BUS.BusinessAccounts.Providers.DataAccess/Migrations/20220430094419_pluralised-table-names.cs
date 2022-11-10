using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Migrations
{
    public partial class pluralisedtablenames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_BusinessAccounts_BusinessAccountId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessAccounts_SubStatuses_SubStatusId",
                table: "BusinessAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_SubStatuses_Statuses_BusinessAccountStatusId",
                table: "SubStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubStatuses",
                table: "SubStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExternalUsers",
                table: "ExternalUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CertificationDetail",
                table: "CertificationDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessAccountStatusHistory",
                table: "BusinessAccountStatusHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.RenameTable(
                name: "UserRole",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "SubStatuses",
                newName: "BusinessAccountSubStatuses");

            migrationBuilder.RenameTable(
                name: "Statuses",
                newName: "BusinessAccountStatuses");

            migrationBuilder.RenameTable(
                name: "ExternalUsers",
                newName: "ExternalUserAccounts");

            migrationBuilder.RenameTable(
                name: "CertificationDetail",
                newName: "CertificationDetails");

            migrationBuilder.RenameTable(
                name: "BusinessAccountStatusHistory",
                newName: "BusinessAccountStatusHistories");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "BusinessAddresses");

            migrationBuilder.RenameIndex(
                name: "IX_SubStatuses_BusinessAccountStatusId",
                table: "BusinessAccountSubStatuses",
                newName: "IX_BusinessAccountSubStatuses_BusinessAccountStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_BusinessAccountId",
                table: "BusinessAddresses",
                newName: "IX_BusinessAddresses_BusinessAccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessAccountSubStatuses",
                table: "BusinessAccountSubStatuses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessAccountStatuses",
                table: "BusinessAccountStatuses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExternalUserAccounts",
                table: "ExternalUserAccounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CertificationDetails",
                table: "CertificationDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessAccountStatusHistories",
                table: "BusinessAccountStatusHistories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessAddresses",
                table: "BusinessAddresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessAccounts_BusinessAccountSubStatuses_SubStatusId",
                table: "BusinessAccounts",
                column: "SubStatusId",
                principalTable: "BusinessAccountSubStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessAccountSubStatuses_BusinessAccountStatuses_BusinessAccountStatusId",
                table: "BusinessAccountSubStatuses",
                column: "BusinessAccountStatusId",
                principalTable: "BusinessAccountStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessAddresses_BusinessAccounts_BusinessAccountId",
                table: "BusinessAddresses",
                column: "BusinessAccountId",
                principalTable: "BusinessAccounts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessAccounts_BusinessAccountSubStatuses_SubStatusId",
                table: "BusinessAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessAccountSubStatuses_BusinessAccountStatuses_BusinessAccountStatusId",
                table: "BusinessAccountSubStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessAddresses_BusinessAccounts_BusinessAccountId",
                table: "BusinessAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExternalUserAccounts",
                table: "ExternalUserAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CertificationDetails",
                table: "CertificationDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessAddresses",
                table: "BusinessAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessAccountSubStatuses",
                table: "BusinessAccountSubStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessAccountStatusHistories",
                table: "BusinessAccountStatusHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessAccountStatuses",
                table: "BusinessAccountStatuses");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRole");

            migrationBuilder.RenameTable(
                name: "ExternalUserAccounts",
                newName: "ExternalUsers");

            migrationBuilder.RenameTable(
                name: "CertificationDetails",
                newName: "CertificationDetail");

            migrationBuilder.RenameTable(
                name: "BusinessAddresses",
                newName: "Addresses");

            migrationBuilder.RenameTable(
                name: "BusinessAccountSubStatuses",
                newName: "SubStatuses");

            migrationBuilder.RenameTable(
                name: "BusinessAccountStatusHistories",
                newName: "BusinessAccountStatusHistory");

            migrationBuilder.RenameTable(
                name: "BusinessAccountStatuses",
                newName: "Statuses");

            migrationBuilder.RenameIndex(
                name: "IX_BusinessAddresses_BusinessAccountId",
                table: "Addresses",
                newName: "IX_Addresses_BusinessAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_BusinessAccountSubStatuses_BusinessAccountStatusId",
                table: "SubStatuses",
                newName: "IX_SubStatuses_BusinessAccountStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExternalUsers",
                table: "ExternalUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CertificationDetail",
                table: "CertificationDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubStatuses",
                table: "SubStatuses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessAccountStatusHistory",
                table: "BusinessAccountStatusHistory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_BusinessAccounts_BusinessAccountId",
                table: "Addresses",
                column: "BusinessAccountId",
                principalTable: "BusinessAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessAccounts_SubStatuses_SubStatusId",
                table: "BusinessAccounts",
                column: "SubStatusId",
                principalTable: "SubStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubStatuses_Statuses_BusinessAccountStatusId",
                table: "SubStatuses",
                column: "BusinessAccountStatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
