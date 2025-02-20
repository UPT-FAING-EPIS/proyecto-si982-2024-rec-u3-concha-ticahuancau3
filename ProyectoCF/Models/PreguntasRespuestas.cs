using System.ComponentModel.DataAnnotations;

namespace ProyectoCF.Models
{
    public class Pregunta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Texto { get; set; }

        [Required]
        public int EvaluacionId { get; set; }

        public Evaluacion Evaluacion { get; set; }

        public ICollection<Respuesta> Respuestas { get; set; } = new List<Respuesta>();
    }

    public class Respuesta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Texto { get; set; }

        public bool EsCorrecta { get; set; }

        [Required]
        public int PreguntaId { get; set; }

        public Pregunta Pregunta { get; set; }
    }
}
