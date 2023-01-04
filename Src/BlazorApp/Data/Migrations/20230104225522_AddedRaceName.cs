using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRaceName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Races",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Races");
        }
    }
}
