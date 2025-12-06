using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductPrediction.API.Migrations
{
    /// <inheritdoc />
    public partial class Adds_title_to_shoppinglist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "shopping_lists",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "shopping_lists");
        }
    }
}
