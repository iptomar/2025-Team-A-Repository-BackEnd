using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class addBlockMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Bloqueado",
                table: "Horarios",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bloqueado",
                table: "Horarios");
        }
    }
}
