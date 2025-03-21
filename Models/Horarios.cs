using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP_Backend.Models
{
    /// <summary>
    /// Tabela de Horários
    /// </summary>
    public class Horarios
    {
        /// <summary>
        /// Construtor da classe Horarios
        /// </summary>
        public Horarios()
        {
            ListaManchasHorarias = new HashSet<ManchasHorarias>();
        }

        [Key]
        public int Id { get; set; }

        public string AnoLetivo { get; set; }

        public string Semestre { get; set; }

        /* ************************************************
         * Vamos criar as Relações (FKs) com outras tabelas
         * *********************************************** */

        // Relacionamento com Turmas (N-1)
        [ForeignKey(nameof(Turma))]
        public int TurmaFK { get; set; }

        public Turmas Turma { get; set; }

        // Relacionamento do tipo N-M, SEM atributos do relacionamento        
        /// <summary>
        /// Lista de Manchas Horárias
        /// </summary>
        public ICollection<ManchasHorarias> ListaManchasHorarias { get; set; }
    }
}
