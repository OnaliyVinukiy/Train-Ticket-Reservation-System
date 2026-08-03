using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRecurringBookingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RecurrenceEndDate",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecurrencePattern",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecurrenceEndDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RecurrencePattern",
                table: "Bookings");
        }
    }
}
