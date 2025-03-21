using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _2025_Team_A_Repository_BackEnd.Models
{
    /// <summary>
    /// Tabela de Turmas
    /// </summary>
    public class Turmas
    {
        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }

        public string AnoLetivo { get; set; }

        /* ************************************************
         * Vamos criar as Relações (FKs) com outras tabelas
         * *********************************************** */

        // Relacionamento do tipo N-1
        [ForeignKey(nameof(Horario))]
        public int HorarioFK { get; set; }

        public Horarios Horario { get; set; }

        // Relacionamento do tipo N-1
        [ForeignKey(nameof(Curso))]
        public int CursoFK { get; set; }

        public Cursos Curso { get; set; }
    }
}
