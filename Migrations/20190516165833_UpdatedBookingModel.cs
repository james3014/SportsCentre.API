using Microsoft.EntityFrameworkCore.Migrations;

namespace SportsCentre.API.Migrations
{
    public partial class UpdatedBookingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBeenPaid",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "BookingType",
                table: "Bookings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Requirements",
                table: "Bookings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingType",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Requirements",
                table: "Bookings");

            migrationBuilder.AddColumn<bool>(
                name: "HasBeenPaid",
                table: "Bookings",
                nullable: false,
                defaultValue: false);
        }
    }
}
