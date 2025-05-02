using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class InsercaoUserID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userID",
                table: "Utilizadores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userID",
                table: "Utilizadores");
        }
    }
}
