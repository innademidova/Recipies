using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipes.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddFavoriteCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FavoritesCount",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavoritesCount",
                table: "Recipes");
        }
    }
}
