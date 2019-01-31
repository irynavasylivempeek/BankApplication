using Microsoft.EntityFrameworkCore.Migrations;

namespace BankApp.DAL.Migrations
{
    public partial class RemovedSalt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Users",
                nullable: false,
                defaultValue: "");
        }
    }
}
