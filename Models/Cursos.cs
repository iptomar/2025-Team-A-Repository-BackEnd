using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace _2025_Team_A_Repository_BackEnd.Models
{
    /// <summary>
    /// Tabela de Cursos
    /// </summary>
    public class Cursos
    {
        /// <summary>
        /// Construtor da classe Cursos
        /// </summary>
        public Cursos()
        {
            ListaUCs = new HashSet<UnidadesCurriculares>();
        }

        [Key]
        public int CodCurso { get; set; }

        public string Nome { get; set; }

        /* ************************************************
         * Vamos criar as Relações (FKs) com outras tabelas
         * *********************************************** */

        // Relacionamento do tipo N-1
        [ForeignKey(nameof(Escola))]
        public int EscolaFK { get; set; }

        public Escolas Escola { get; set; }

        // Relacionamento do tipo N-M, SEM atributos do relacionamento        
        /// <summary>
        /// Lista de UCs
        /// </summary>
        public ICollection<UnidadesCurriculares> ListaUCs { get; set; }
    }
}
