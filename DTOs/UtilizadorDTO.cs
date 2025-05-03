namespace GP_Backend.DTOs
{
    public class UtilizadorDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? EscolaNome { get; set; }
        public string? CursoNome { get; set; }

        public bool EmailConfirmed { get; set; }

        public string? Email { get; set; }

        public string? Role { get; set; }

        public string UserID { get; set; }


    }
}
