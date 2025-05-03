using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Interasian.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PropertyType",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ListingId",
                keyValue: 1,
                columns: new[] { "Owner", "PropertyType" },
                values: new object[] { "John Smith", "House with Lot" });

            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ListingId",
                keyValue: 2,
                columns: new[] { "Owner", "PropertyType" },
                values: new object[] { "Jane Doe", "Condo" });

            migrationBuilder.InsertData(
                table: "Listings",
                columns: new[] { "ListingId", "BathRooms", "BedRooms", "Description", "FloorArea", "LandArea", "Location", "Owner", "Price", "PropertyType", "Status", "Title" },
                values: new object[,]
                {
                    { 3, 8, null, "Prime commercial building with 5 floors suitable for office spaces.", "3500 sqm", "1200 sqm", "Ortigas Center", "Metro Properties Inc.", 75000000m, "Commercial Building", true, "Ayala Building" },
                    { 4, null, null, "Beautiful vacant lot with panoramic view of Taal Lake.", null, "800 sqm", "Lapu-Lapu City", "Connie Tangpuz", 6500000m, "House with Lot", true, "Tangpuz House" },
                    { 5, 4, 5, "Spacious family home in a gated community with garden and garage.", "280 sqm", "350 sqm", "Alabang, Muntinlupa", "Maria Santos", 18500000m, "House with Lot", true, "Suburban Family Home" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Listings",
                keyColumn: "ListingId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Listings",
                keyColumn: "ListingId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Listings",
                keyColumn: "ListingId",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "PropertyType",
                table: "Listings");
        }
    }
}
