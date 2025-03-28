using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP_Backend.Models
{
    /// <summary>
    /// Tabela de Escolas
    /// </summary>
    public class Escolas
    {
        /// <summary>
        /// Construtor da classe Escolas
        /// </summary>
        public Escolas()
        {
            ListaCursos = new HashSet<Cursos>();
            ListaSalas = new HashSet<Salas>();
        }

        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }

        /* ************************************************
         * Vamos criar as Relações (FKs) com outras tabelas
         * *********************************************** */

        // Relacionamento com Cursos
        /// <summary>
        /// Lista dos Cursos associados à escola
        /// </summary>
        public ICollection<Cursos> ListaCursos { get; set; }

        // Relacionamento com Salas
        /// <summary>
        /// Lista das Salas associados à escola
        /// </summary>
        public ICollection<Salas> ListaSalas { get; set; }
    }
}