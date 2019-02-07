using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankApp.DAL.Migrations
{
    public partial class RowVersionAndConcurencyToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Salt",
                table: "Users",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "Accounts",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Accounts");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Salt",
                table: "Users",
                nullable: true,
                oldClrType: typeof(byte[]));
        }
    }
}
