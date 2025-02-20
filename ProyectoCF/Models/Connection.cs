using Microsoft.EntityFrameworkCore;

namespace ProyectoCF.Models
{
    public class Connection : DbContext
    {
        public Connection(DbContextOptions<Connection> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<EstudianteCurso> EstudiantesCursos { get; set; }
        public DbSet<Material> Materiales { get; set; }
        public DbSet<Evaluacion> Evaluaciones { get; set; }
        public DbSet<Nota> Notas { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }
    }
}
