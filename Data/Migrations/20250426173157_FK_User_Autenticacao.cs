using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class FK_User_Autenticacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Utilizadores");

            migrationBuilder.AlterColumn<int>(
                name: "EscolaFK",
                table: "Utilizadores",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CursoFK",
                table: "Utilizadores",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Utilizadores",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_Cursos_CursoFK",
                table: "Utilizadores",
                column: "CursoFK",
                principalTable: "Cursos",
                principalColumn: "CodCurso");

           
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

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Utilizadores");

            migrationBuilder.AlterColumn<int>(
                name: "EscolaFK",
                table: "Utilizadores",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CursoFK",
                table: "Utilizadores",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Utilizadores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Utilizadores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Utilizadores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_Cursos_CursoFK",
                table: "Utilizadores",
                column: "CursoFK",
                principalTable: "Cursos",
                principalColumn: "CodCurso",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_Escolas_EscolaFK",
                table: "Utilizadores",
                column: "EscolaFK",
                principalTable: "Escolas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
