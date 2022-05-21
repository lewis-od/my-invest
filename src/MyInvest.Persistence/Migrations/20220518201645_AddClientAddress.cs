using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyInvest.Migrations
{
    public partial class AddClientAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "address_is_verified",
                table: "clients",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "address_line_1",
                table: "clients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "address_line_2",
                table: "clients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "address_postcode",
                table: "clients",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address_is_verified",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "address_line_1",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "address_line_2",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "address_postcode",
                table: "clients");
        }
    }
}
