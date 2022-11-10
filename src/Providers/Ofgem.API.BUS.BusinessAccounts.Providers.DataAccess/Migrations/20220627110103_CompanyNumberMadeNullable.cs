using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Migrations
{
    public partial class CompanyNumberMadeNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CompanyNumber",
                table: "CompaniesHouseDetails",
                type: "varchar(8)",
                unicode: false,
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(8)",
                oldUnicode: false,
                oldMaxLength: 8);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CompanyNumber",
                table: "CompaniesHouseDetails",
                type: "varchar(8)",
                unicode: false,
                maxLength: 8,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(8)",
                oldUnicode: false,
                oldMaxLength: 8,
                oldNullable: true);
        }
    }
}
