using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP_Backend.Models
{
    /// <summary>
    /// Tabela de Salas
    /// </summary>
    public class Salas {

		[Key]
		public int Id { get; set; }

		public string Nome { get; set; }

		
		/* ************************************************
         * Vamos criar as Relações (FKs) com outras tabelas
         * *********************************************** */

		// Relacionamento do tipo N-1
		[ForeignKey(nameof(Escola))]
		public int EscolaFK { get; set; }

		public Escolas Escola { get; set; }

}
}