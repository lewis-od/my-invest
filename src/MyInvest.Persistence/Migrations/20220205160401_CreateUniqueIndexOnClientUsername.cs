using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyInvest.Migrations
{
    public partial class CreateUniqueIndexOnClientUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_clients_username",
                table: "clients",
                column: "username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_clients_username",
                table: "clients");
        }
    }
}
