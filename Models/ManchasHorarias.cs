namespace _2025-Team-A-Repository-BackEnd.Models
{
    /// <summary>
    /// Tabela de Manchas Horárias (Aulas)
    /// </summary>
    public class ManchasHorarias {

        /// <summary>
        /// Construtor da classe ManchasHorarias
        /// </summary>
        public ManchasHorarias()
        {
            ListaHorarios = new HashSet<Horarios>();
        }

        [Key]
        public int Id { get; set; }

        public string TipoDeAula { get; set; }

        public int NumSlots { get; set; }

        /* ************************************************
         * Vamos criar as Relações (FKs) com outras tabelas
         * *********************************************** */

        // Relacionamento do tipo N-1
        [ForeignKey(nameof(Docente))]
        public int DocenteFK { get; set; }

        public Docentes Docente { get; set; }


        // Relacionamento do tipo N-1
        [ForeignKey(nameof(Sala))]
        public int SalaFK { get; set; }

        public Salas Sala { get; set; }

        // Relacionamento do tipo N-1
        [ForeignKey(nameof(UC))]
        public int UCFK { get; set; }

        public UnidadesCurriculares UC { get; set; }


        // Relacionamento do tipo N-M, SEM atributos do relacionamento        
        /// <summary>
        /// Lista de Horarios
        /// </summary>
        public ICollection<Horarios> ListaHorarios { get; set; }


}
}