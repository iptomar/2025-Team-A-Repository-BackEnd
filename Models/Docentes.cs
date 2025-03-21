using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP_Backend.Models
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
