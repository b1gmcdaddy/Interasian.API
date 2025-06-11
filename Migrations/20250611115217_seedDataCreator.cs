using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Interasian.API.Migrations
{
    /// <inheritdoc />
    public partial class seedDataCreator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ListingId",
                keyValue: 1,
                column: "Creator",
                value: "jtangpuz@guardian.ph");

            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ListingId",
                keyValue: 2,
                column: "Creator",
                value: "jtangpuz@guardian.ph");

            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ListingId",
                keyValue: 3,
                column: "Creator",
                value: "jtangpuz@guardian.ph");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ListingId",
                keyValue: 1,
                column: "Creator",
                value: "");

            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ListingId",
                keyValue: 2,
                column: "Creator",
                value: "");

            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ListingId",
                keyValue: 3,
                column: "Creator",
                value: "");
        }
    }
}
