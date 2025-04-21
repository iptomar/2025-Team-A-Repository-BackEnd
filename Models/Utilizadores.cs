using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP_Backend.Models
{
    /// <summary>
    /// Tabela de Utilizadores
    /// </summary>
    public class Utilizadores : IdentityUser{

        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }


        /* ************************************************
         * Vamos criar as Relações (FKs) com outras tabelas
         * *********************************************** */

        // Relacionamento do tipo N-1
        [ForeignKey(nameof(Escola))]
        public int EscolaFK { get; set; }

        public Escolas Escola { get; set; }

        // Relacionamento do tipo N-1
        [ForeignKey(nameof(Curso))]
        public int CursoFK { get; set; }

        public Cursos Curso { get; set; }

}
}