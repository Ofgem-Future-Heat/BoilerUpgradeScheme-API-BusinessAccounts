using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Migrations
{
    public partial class createbusinessaccountdatabasemopupr01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ExternalUserAccounts",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ExternalUserAccounts",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "TradingName",
                table: "BusinessAccounts",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "BankAccounts",
                type: "varchar(155)",
                unicode: false,
                maxLength: 155,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ExternalUserAccounts");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ExternalUserAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "TradingName",
                table: "BusinessAccounts",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "BankAccounts",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(155)",
                oldUnicode: false,
                oldMaxLength: 155);
        }
    }
}
