using GP_Backend.DTOs.Import;
using GP_Backend.Models;

namespace GP_Backend.DTOs.Import
{
    public class ImportDto
    {
        public List<EscolaDto> ListaEscolas { get; set; }
        public List<DocenteDto> ListaDocentes { get; set; }
        public List<CursoDto> ListaCursos { get; set; }
        public List<SalaDto> ListaSalas { get; set; }
        public List<UnidadeCurricularDto> ListaUCs { get; set; }
    }
}
