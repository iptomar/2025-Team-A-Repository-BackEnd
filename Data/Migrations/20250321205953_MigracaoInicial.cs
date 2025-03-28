using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Docentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Docentes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Escolas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escolas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Plano = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Semestre = table.Column<int>(type: "int", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UCs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cursos",
                columns: table => new
                {
                    CodCurso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EscolaFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursos", x => x.CodCurso);
                    table.ForeignKey(
                        name: "FK_Cursos_Escolas_EscolaFK",
                        column: x => x.EscolaFK,
                        principalTable: "Escolas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Salas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EscolaFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Salas_Escolas_EscolaFK",
                        column: x => x.EscolaFK,
                        principalTable: "Escolas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CursosUnidadesCurriculares",
                columns: table => new
                {
                    ListaCursosCodCurso = table.Column<int>(type: "int", nullable: false),
                    ListaUCsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CursosUnidadesCurriculares", x => new { x.ListaCursosCodCurso, x.ListaUCsId });
                    table.ForeignKey(
                        name: "FK_CursosUnidadesCurriculares_Cursos_ListaCursosCodCurso",
                        column: x => x.ListaCursosCodCurso,
                        principalTable: "Cursos",
                        principalColumn: "CodCurso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CursosUnidadesCurriculares_UCs_ListaUCsId",
                        column: x => x.ListaUCsId,
                        principalTable: "UCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EscolaFK = table.Column<int>(type: "int", nullable: false),
                    CursoFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Utilizadores_Cursos_CursoFK",
                        column: x => x.CursoFK,
                        principalTable: "Cursos",
                        principalColumn: "CodCurso",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Utilizadores_Escolas_EscolaFK",
                        column: x => x.EscolaFK,
                        principalTable: "Escolas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ManchasHorarias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoDeAula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumSlots = table.Column<int>(type: "int", nullable: false),
                    DocenteFK = table.Column<int>(type: "int", nullable: false),
                    SalaFK = table.Column<int>(type: "int", nullable: false),
                    UCFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManchasHorarias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManchasHorarias_Docentes_DocenteFK",
                        column: x => x.DocenteFK,
                        principalTable: "Docentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManchasHorarias_Salas_SalaFK",
                        column: x => x.SalaFK,
                        principalTable: "Salas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManchasHorarias_UCs_UCFK",
                        column: x => x.UCFK,
                        principalTable: "UCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnoLetivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Semestre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TurmaFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorariosManchasHorarias",
                columns: table => new
                {
                    ListaHorariosId = table.Column<int>(type: "int", nullable: false),
                    ListaManchasHorariasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosManchasHorarias", x => new { x.ListaHorariosId, x.ListaManchasHorariasId });
                    table.ForeignKey(
                        name: "FK_HorariosManchasHorarias_Horarios_ListaHorariosId",
                        column: x => x.ListaHorariosId,
                        principalTable: "Horarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HorariosManchasHorarias_ManchasHorarias_ListaManchasHorariasId",
                        column: x => x.ListaManchasHorariasId,
                        principalTable: "ManchasHorarias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turmas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnoLetivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HorarioFK = table.Column<int>(type: "int", nullable: false),
                    CursoFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turmas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Turmas_Cursos_CursoFK",
                        column: x => x.CursoFK,
                        principalTable: "Cursos",
                        principalColumn: "CodCurso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Turmas_Horarios_HorarioFK",
                        column: x => x.HorarioFK,
                        principalTable: "Horarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_EscolaFK",
                table: "Cursos",
                column: "EscolaFK");

            migrationBuilder.CreateIndex(
                name: "IX_CursosUnidadesCurriculares_ListaUCsId",
                table: "CursosUnidadesCurriculares",
                column: "ListaUCsId");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_TurmaFK",
                table: "Horarios",
                column: "TurmaFK");

            migrationBuilder.CreateIndex(
                name: "IX_HorariosManchasHorarias_ListaManchasHorariasId",
                table: "HorariosManchasHorarias",
                column: "ListaManchasHorariasId");

            migrationBuilder.CreateIndex(
                name: "IX_ManchasHorarias_DocenteFK",
                table: "ManchasHorarias",
                column: "DocenteFK");

            migrationBuilder.CreateIndex(
                name: "IX_ManchasHorarias_SalaFK",
                table: "ManchasHorarias",
                column: "SalaFK");

            migrationBuilder.CreateIndex(
                name: "IX_ManchasHorarias_UCFK",
                table: "ManchasHorarias",
                column: "UCFK");

            migrationBuilder.CreateIndex(
                name: "IX_Salas_EscolaFK",
                table: "Salas",
                column: "EscolaFK");

            migrationBuilder.CreateIndex(
                name: "IX_Turmas_CursoFK",
                table: "Turmas",
                column: "CursoFK");

            migrationBuilder.CreateIndex(
                name: "IX_Turmas_HorarioFK",
                table: "Turmas",
                column: "HorarioFK");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_CursoFK",
                table: "Utilizadores",
                column: "CursoFK");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_EscolaFK",
                table: "Utilizadores",
                column: "EscolaFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Horarios_Turmas_TurmaFK",
                table: "Horarios",
                column: "TurmaFK",
                principalTable: "Turmas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cursos_Escolas_EscolaFK",
                table: "Cursos");

            migrationBuilder.DropForeignKey(
                name: "FK_Turmas_Cursos_CursoFK",
                table: "Turmas");

            migrationBuilder.DropForeignKey(
                name: "FK_Horarios_Turmas_TurmaFK",
                table: "Horarios");

            migrationBuilder.DropTable(
                name: "CursosUnidadesCurriculares");

            migrationBuilder.DropTable(
                name: "HorariosManchasHorarias");

            migrationBuilder.DropTable(
                name: "Utilizadores");

            migrationBuilder.DropTable(
                name: "ManchasHorarias");

            migrationBuilder.DropTable(
                name: "Docentes");

            migrationBuilder.DropTable(
                name: "Salas");

            migrationBuilder.DropTable(
                name: "UCs");

            migrationBuilder.DropTable(
                name: "Escolas");

            migrationBuilder.DropTable(
                name: "Cursos");

            migrationBuilder.DropTable(
                name: "Turmas");

            migrationBuilder.DropTable(
                name: "Horarios");
        }
    }
}
