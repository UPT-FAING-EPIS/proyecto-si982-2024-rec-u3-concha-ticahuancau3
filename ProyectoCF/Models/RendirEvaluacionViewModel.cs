using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoCF.Models
{
    public class RendirEvaluacionViewModel
    {
        public int EvaluacionId { get; set; }
        public string Titulo { get; set; }
        public List<PreguntaRespuestaViewModel> Preguntas { get; set; }
    }

    public class PreguntaRespuestaViewModel
    {
        public int PreguntaId { get; set; }
        public string Texto { get; set; }
        public List<RespuestaOption> Respuestas { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una respuesta.")]
        public int? RespuestaSeleccionada { get; set; }
    }

    public class RespuestaOption
    {
        public int RespuestaId { get; set; }
        public string Texto { get; set; }
    }
}
