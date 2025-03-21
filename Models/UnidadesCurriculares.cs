using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace GP_Backend.Models
{
    /// <summary>
    /// Tabela de Unidades Curriculares
    /// </summary>
    public class UnidadesCurriculares
    {
        /// <summary>
        /// Construtor da classe UnidadesCurriculares
        /// </summary>
        public UnidadesCurriculares()
        {
             ListaCursos = new HashSet<Cursos>();
        }

        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Plano { get; set; }

        public int Semestre { get; set; }

        public int Ano { get; set; }

        /* ************************************************
         * Vamos criar as Relações (FKs) com outras tabelas
         * *********************************************** */

        // Relacionamento do tipo N-M, SEM atributos do relacionamento        
        /// <summary>
        /// Lista de UCs
        /// </summary>
        public ICollection<Cursos> ListaCursos { get; set; }

    }
}