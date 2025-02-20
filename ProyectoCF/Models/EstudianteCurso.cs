using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoCF.Models
{
    public class EstudianteCurso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EstudianteId { get; set; }

        [Required]
        public int CursoId { get; set; }

        [ForeignKey("EstudianteId")]
        public Usuario Estudiante { get; set; }

        [ForeignKey("CursoId")]
        public Curso Curso { get; set; }
    }
}
