using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blogSitesi.Migrations
{
    /// <inheritdoc />
    public partial class seedBlogpostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BlogPosts",
                columns: new[] { "Id", "Content", "CreatedDate", "ImagePath", "Title", "UpdatedDate" },
                values: new object[] { 1, "Evren yıllardır sıragelen sırlar tohumunun bir özüdür", new DateTime(2024, 7, 31, 15, 29, 49, 40, DateTimeKind.Local).AddTicks(6601), "https://picsum.photos/200", "Evrenin sırrı", new DateTime(2024, 7, 31, 15, 29, 49, 40, DateTimeKind.Local).AddTicks(6602) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
