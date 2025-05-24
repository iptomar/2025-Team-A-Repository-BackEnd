using GP_Backend.Models;
using static GP_Backend.Controllers.API_ManchasHorariasController;

namespace GP_Backend.DTOs
{
    public class ImportDto
    {
        public List<Escolas> ListaEscolas { get; set; }
        public List<Docentes> ListaDocentes { get; set; }
        public List<Cursos> ListaCursos { get; set; }
        public List<Salas> ListaSalas { get; set; }
        public List<UnidadesCurriculares> ListaUCs { get; set; }
    }
}
