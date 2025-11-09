using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoManager.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageBase64",
                table: "Vehicles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageBase64",
                table: "Vehicles",
                type: "text",
                nullable: true);
        }
    }
}
