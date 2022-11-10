using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Migrations
{
    public partial class InviteStatuses_Creation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StatusID",
                table: "Invites",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "InviteStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InviteStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invites_StatusID",
                table: "Invites",
                column: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Invites_InviteStatuses_StatusID",
                table: "Invites",
                column: "StatusID",
                principalTable: "InviteStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invites_InviteStatuses_StatusID",
                table: "Invites");

            migrationBuilder.DropTable(
                name: "InviteStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Invites_StatusID",
                table: "Invites");

            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "Invites");
        }
    }
}
