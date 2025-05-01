using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Interasian.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    ListingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LandArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FloorArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BedRooms = table.Column<int>(type: "int", nullable: true),
                    BathRooms = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.ListingId);
                });

            migrationBuilder.InsertData(
                table: "Listings",
                columns: new[] { "ListingId", "BathRooms", "BedRooms", "Description", "FloorArea", "LandArea", "Location", "Price", "Status", "Title" },
                values: new object[,]
                {
                    { 1, 3, 4, "A luxurious villa with ocean view and private pool.", "350 sqm", "500 sqm", "Boracay Island", 15000000m, true, "Beachside Villa" },
                    { 2, 1, 2, "Modern condo in the heart of the business district.", "60 sqm", "60 sqm", "Makati City", 4500000m, true, "Urban Condo" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Listings");
        }
    }
}
