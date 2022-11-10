using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Migrations
{
    public partial class InviteCreationUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Invite",
                table: "Invite");

            migrationBuilder.RenameTable(
                name: "Invite",
                newName: "Invites");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invites",
                table: "Invites",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_ExternalUserAccountId",
                table: "Invites",
                column: "ExternalUserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_ExternalUserAccounts_ExternalUserAccountId",
                table: "Invites",
                column: "ExternalUserAccountId",
                principalTable: "ExternalUserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_ExternalUserAccounts_ExternalUserAccountId",
                table: "Invites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invites",
                table: "Invites");

            migrationBuilder.DropIndex(
                name: "IX_Invites_ExternalUserAccountId",
                table: "Invites");

            migrationBuilder.RenameTable(
                name: "Invites",
                newName: "Invite");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invite",
                table: "Invite",
                column: "ID");
        }
    }
}
