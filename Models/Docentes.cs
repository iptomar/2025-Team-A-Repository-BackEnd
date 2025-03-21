using System.ComponentModel.DataAnnotations;

namespace _2025_Team_A_Repository_BackEnd.Models
{
    /// <summary>
    /// Tabela de Docentes
    /// </summary>
    public class Docentes
    {
        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }
    }
}
