using System.ComponentModel.DataAnnotations;

namespace ProyectoCF.Models
{
    public class PreguntaCreateViewModel
    {
        public int EvaluacionId { get; set; }

        [Required(ErrorMessage = "El texto de la pregunta es obligatorio.")]
        public string Texto { get; set; }

        [Required(ErrorMessage = "La respuesta 1 es obligatoria.")]
        public string Respuesta1 { get; set; }

        [Required(ErrorMessage = "La respuesta 2 es obligatoria.")]
        public string Respuesta2 { get; set; }

        [Required(ErrorMessage = "La respuesta 3 es obligatoria.")]
        public string Respuesta3 { get; set; }

        [Required(ErrorMessage = "Debe seleccionar cuál respuesta es la correcta.")]
        [Range(1, 3, ErrorMessage = "Seleccione una respuesta válida.")]
        public int CorrectaIndex { get; set; }
    }
}
