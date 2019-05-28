using Microsoft.EntityFrameworkCore.Migrations;

namespace SportsCentre.API.Migrations
{
    public partial class UpdatedClassModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Facility",
                table: "Classes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Facility",
                table: "Classes");
        }
    }
}
