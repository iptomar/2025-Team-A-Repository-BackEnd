using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP_Backend.Models
{
    /// <summary>
    /// Tabela de Utilizadores
    /// </summary>
    public class Utilizadores
    {

        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }


        /* ************************************************
         * Vamos criar as Relações (FKs) com outras tabelas
         * *********************************************** */

        // Relacionamento do tipo N-1
        [ForeignKey(nameof(Escola))]
        public int? EscolaFK { get; set; }

        public Escolas? Escola { get; set; }

        // Relacionamento do tipo N-1
        [ForeignKey(nameof(Curso))]
        public int? CursoFK { get; set; }

        public Cursos? Curso { get; set; }

        /// <summary>
        /// atributo para funcionar como FK
        /// no relacionamento entre a 
        /// base de dados do 'negócio' 
        /// e a base de dados da 'autenticação'
        /// </summary>

        public string UserID { get; set; }

    }
}