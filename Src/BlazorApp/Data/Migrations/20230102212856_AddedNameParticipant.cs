using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedNameParticipant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Participants",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Participants");
        }
    }
}
