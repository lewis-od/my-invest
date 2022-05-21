using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyInvest.Migrations
{
    public partial class CreateInvestmentAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "investment_accounts",
                columns: table => new
                {
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    client_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    balance = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_investment_accounts", x => x.account_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_investment_accounts_client_id",
                table: "investment_accounts",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_investment_accounts_client_id_type",
                table: "investment_accounts",
                columns: new[] { "client_id", "type" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "investment_accounts");
        }
    }
}
