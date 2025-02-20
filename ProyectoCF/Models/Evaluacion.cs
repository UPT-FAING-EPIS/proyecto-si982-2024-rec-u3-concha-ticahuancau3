using System.ComponentModel.DataAnnotations;

namespace ProyectoCF.Models
{
    public class Evaluacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public int CursoId { get; set; }

        public Curso Curso { get; set; }

        public ICollection<Pregunta> Preguntas { get; set; } = new List<Pregunta>();
    }
}
