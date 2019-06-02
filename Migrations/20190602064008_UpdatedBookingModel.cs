using Microsoft.EntityFrameworkCore.Migrations;

namespace SportsCentre.API.Migrations
{
    public partial class UpdatedBookingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Bookings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facility",
                table: "Bookings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Facility",
                table: "Bookings");
        }
    }
}
