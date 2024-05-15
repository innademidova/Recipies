using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipes.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsBanned : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "UserAccount",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "UserAccount");
        }
    }
}
