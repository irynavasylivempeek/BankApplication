using Microsoft.EntityFrameworkCore.Migrations;

namespace BankApp.DAL.Migrations
{
    public partial class ChangedTransactionsAndNamesOfProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_DestinationId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "Login",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Transactions",
                newName: "SenderAccountId");

            migrationBuilder.RenameColumn(
                name: "DestinationId",
                table: "Transactions",
                newName: "ReceiverAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_DestinationId",
                table: "Transactions",
                newName: "IX_Transactions_ReceiverAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                newName: "IX_Transactions_SenderAccountId");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Transactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_ReceiverAccountId",
                table: "Transactions",
                column: "ReceiverAccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_SenderAccountId",
                table: "Transactions",
                column: "SenderAccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_ReceiverAccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_SenderAccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "Login");

            migrationBuilder.RenameColumn(
                name: "SenderAccountId",
                table: "Transactions",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "ReceiverAccountId",
                table: "Transactions",
                newName: "DestinationId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SenderAccountId",
                table: "Transactions",
                newName: "IX_Transactions_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_ReceiverAccountId",
                table: "Transactions",
                newName: "IX_Transactions_DestinationId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Transactions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_DestinationId",
                table: "Transactions",
                column: "DestinationId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
