using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PixerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldMoneyToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "money",
                schema: "pixer_database",
                table: "users",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "money",
                schema: "pixer_database",
                table: "users");
        }
    }
}
