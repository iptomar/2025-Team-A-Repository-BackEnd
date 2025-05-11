namespace GP_Backend.DTOs
{
    public class UtilizadorUpdateDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? RoleId { get; set; }

        public int? EscolaId { get; set; }

        public int? CodCurso { get; set; }
    }
}
