using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoInicial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turmas_Horarios_HorarioFK",
                table: "Turmas");

            migrationBuilder.DropIndex(
                name: "IX_Turmas_HorarioFK",
                table: "Turmas");

            migrationBuilder.DropColumn(
                name: "HorarioFK",
                table: "Turmas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HorarioFK",
                table: "Turmas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Turmas_HorarioFK",
                table: "Turmas",
                column: "HorarioFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Turmas_Horarios_HorarioFK",
                table: "Turmas",
                column: "HorarioFK",
                principalTable: "Horarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
