namespace GP_Backend.DTOs
{
    public class UnidadeCurricularDto
    {
        public string Nome { get; set; }
        public string Plano { get; set; }
        public int Semestre { get; set; }
        public int Ano { get; set; }
        public List<CursoDto> ListaCursos { get; set; }
    }
}
