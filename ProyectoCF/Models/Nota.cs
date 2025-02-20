using System.ComponentModel.DataAnnotations;

namespace ProyectoCF.Models
{
    public class Nota
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EstudianteId { get; set; }

        [Required]
        public int EvaluacionId { get; set; }

        [Required]
        public decimal Calificacion { get; set; }

        public Usuario Estudiante { get; set; }

        public Evaluacion Evaluacion { get; set; }
    }
}
