namespace ProyectoCF.Models
{
    public class EvaluacionViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string CursoNombre { get; set; } // Nombre del curso asociado
        public bool YaRendida { get; set; } // Indica si el estudiante ya rindió la evaluación
    }
}