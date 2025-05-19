using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Interasian.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "138108e9-9724-4370-9c51-aaf7a7c1181c", "d7b9fd3f-b505-45ce-9bf5-aff8ac3b4501", "Admin", "ADMIN" },
                    { "c6c0e88e-8958-4a9e-9f0f-5a1dba72bdcd", "6c4ef8df-0439-42ba-a486-99182fd45276", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "138108e9-9724-4370-9c51-aaf7a7c1181c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c6c0e88e-8958-4a9e-9f0f-5a1dba72bdcd");
        }
    }
}
