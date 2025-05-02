using System.ComponentModel.DataAnnotations;

namespace GP_Backend.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public int EscolaFK { get; set; }

        [Required]
        public int CursoFK { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
