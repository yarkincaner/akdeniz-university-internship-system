using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Internships.Infrastructure.Migrations
{
    public partial class IsEnabledtoInternshipStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "InternshipStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "InternshipStatuses");
        }
    }
}
