using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internships.Infrastructure.Migrations
{
    public partial class UpdateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BirthYear",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "TcKimlikNo",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthYear",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TcKimlikNo",
                table: "Users");
        }
    }
}
