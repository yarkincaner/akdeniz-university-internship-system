using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internships.Infrastructure.Migrations
{
    public partial class AddTotalDaysAndInsuranceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InsuranceType",
                table: "Internships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalDays",
                table: "Internships",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsuranceType",
                table: "Internships");

            migrationBuilder.DropColumn(
                name: "TotalDays",
                table: "Internships");
        }
    }
}
