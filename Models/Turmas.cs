using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP_Backend.Models
{
    /// <summary>
    /// Tabela de Turmas
    /// </summary>
    public class Turmas
    {
        /// <summary>
        /// Construtor da classe Turmas
        /// </summary>
        public Turmas()
        {
            Horarios = new HashSet<Horarios>();
        }


        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }

        public string AnoLetivo { get; set; }

        /* ************************************************
         * Vamos criar as Relações (FKs) com outras tabelas
         * *********************************************** */

        /* ************************************************
         * Relacionamento 1-N com Horarios
         * Uma turma pode ter vários horários
         * *********************************************** */
        public ICollection<Horarios> Horarios { get; set; }

        // Relacionamento do tipo N-1
        [ForeignKey(nameof(Curso))]
        public int CursoFK { get; set; }

        public Cursos Curso { get; set; }
    }
}
