using Microsoft.EntityFrameworkCore.Migrations;

namespace SportsCentre.API.Migrations
{
    public partial class AttendeesAddedBookingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attendees",
                table: "Bookings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attendees",
                table: "Bookings");
        }
    }
}
