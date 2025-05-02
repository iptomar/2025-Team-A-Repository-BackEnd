using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class RestruturacaoUtilizadores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CursoFK",
                table: "Utilizadores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EscolaFK",
                table: "Utilizadores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Utilizadores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "userID",
                table: "Utilizadores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_CursoFK",
                table: "Utilizadores",
                column: "CursoFK");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_EscolaFK",
                table: "Utilizadores",
                column: "EscolaFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_Cursos_CursoFK",
                table: "Utilizadores",
                column: "CursoFK",
                principalTable: "Cursos",
                principalColumn: "CodCurso");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_Escolas_EscolaFK",
                table: "Utilizadores",
                column: "EscolaFK",
                principalTable: "Escolas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_Cursos_CursoFK",
                table: "Utilizadores");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_Escolas_EscolaFK",
                table: "Utilizadores");

            migrationBuilder.DropIndex(
                name: "IX_Utilizadores_CursoFK",
                table: "Utilizadores");

            migrationBuilder.DropIndex(
                name: "IX_Utilizadores_EscolaFK",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "CursoFK",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "EscolaFK",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "userID",
                table: "Utilizadores");
        }
    }
}
