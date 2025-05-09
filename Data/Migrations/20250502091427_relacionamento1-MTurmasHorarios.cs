using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class relacionamento1MTurmasHorarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnoLetivo",
                table: "Turmas",
                newName: "AnoCurso");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnoCurso",
                table: "Turmas",
                newName: "AnoLetivo");
        }
    }
}
