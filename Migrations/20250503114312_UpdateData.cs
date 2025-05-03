using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Interasian.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ListingId",
                keyValue: 3,
                column: "BedRooms",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ListingId",
                keyValue: 4,
                columns: new[] { "BathRooms", "BedRooms" },
                values: new object[] { 5, 5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ListingId",
                keyValue: 3,
                column: "BedRooms",
                value: null);

            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ListingId",
                keyValue: 4,
                columns: new[] { "BathRooms", "BedRooms" },
                values: new object[] { null, null });
        }
    }
}
