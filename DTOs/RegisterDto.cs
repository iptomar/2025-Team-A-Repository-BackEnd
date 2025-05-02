using System.ComponentModel.DataAnnotations;

namespace GP_Backend.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Escola { get; set; }

        [Required]
        public string Curso { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
