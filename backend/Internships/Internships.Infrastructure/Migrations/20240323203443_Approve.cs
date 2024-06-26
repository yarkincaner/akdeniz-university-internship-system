using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internships.Infrastructure.Migrations
{
    public partial class Approve : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "Internships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Internships",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Internships");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Internships");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Companies");
        }
    }
}
