using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldDescriptionToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                schema: "pixer_database",
                table: "users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                schema: "pixer_database",
                table: "users");
        }
    }
}
