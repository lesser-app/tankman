using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tankman.Migrations
{
    /// <inheritdoc />
    public partial class AddActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "active",
                table: "orgs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "active",
                table: "orgs");
        }
    }
}
