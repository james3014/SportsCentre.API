using Microsoft.EntityFrameworkCore.Migrations;

namespace SportsCentre.API.Migrations
{
    public partial class UpdatedClassModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClassTime",
                table: "Classes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassTime",
                table: "Classes");
        }
    }
}
