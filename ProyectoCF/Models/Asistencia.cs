using System.ComponentModel.DataAnnotations;

namespace ProyectoCF.Models
{
    public class Asistencia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EstudianteId { get; set; }

        [Required]
        public int CursoId { get; set; }

        [Required]
        public bool Presente { get; set; }

        public Usuario Estudiante { get; set; }

        public Curso Curso { get; set; }
    }
}
