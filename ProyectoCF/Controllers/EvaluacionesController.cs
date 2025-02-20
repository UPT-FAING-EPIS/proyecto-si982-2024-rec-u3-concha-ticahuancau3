using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoCF.Models;

namespace ProyectoCF.Controllers
{
    public class EvaluacionesController : Controller
    {
        private readonly Connection _context;

        public EvaluacionesController(Connection context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var usuarioId = int.Parse(HttpContext.Session.GetString("UsuarioId"));
            var rol = HttpContext.Session.GetString("Rol");
            ViewBag.Rol = rol;

            List<EvaluacionViewModel> evaluacionesViewModel = new List<EvaluacionViewModel>();

            if (rol == "Administrador")
            {
                evaluacionesViewModel = _context.Evaluaciones
                    .Include(e => e.Curso)
                    .Select(e => new EvaluacionViewModel
                    {
                        Id = e.Id,
                        Titulo = e.Titulo,
                        Descripcion = e.Descripcion,
                        CursoNombre = e.Curso.Nombre,
                        YaRendida = false // Los administradores no tienen evaluaciones rendidas
                    })
                    .ToList();
            }
            else if (rol == "Docente")
            {
                evaluacionesViewModel = _context.Evaluaciones
                    .Include(e => e.Curso)
                    .Where(e => e.Curso.DocenteId == usuarioId)
                    .Select(e => new EvaluacionViewModel
                    {
                        Id = e.Id,
                        Titulo = e.Titulo,
                        Descripcion = e.Descripcion,
                        CursoNombre = e.Curso.Nombre,
                        YaRendida = false // Los docentes no tienen evaluaciones rendidas
                    })
                    .ToList();
            }
            else if (rol == "Estudiante")
            {
                var evaluaciones = (from e in _context.Evaluaciones.Include(e => e.Curso)
                                    join ec in _context.EstudiantesCursos
                                      on e.CursoId equals ec.CursoId
                                    where ec.EstudianteId == usuarioId
                                    select e)
                                    .Distinct()
                                    .ToList();

                evaluacionesViewModel = evaluaciones.Select(e => new EvaluacionViewModel
                {
                    Id = e.Id,
                    Titulo = e.Titulo,
                    Descripcion = e.Descripcion,
                    CursoNombre = e.Curso.Nombre,
                    YaRendida = _context.Notas.Any(n => n.EvaluacionId == e.Id && n.EstudianteId == usuarioId)
                }).ToList();
            }

            return View(evaluacionesViewModel);
        }

        public IActionResult Create()
        {
            var rol = HttpContext.Session.GetString("Rol");
            var usuarioId = int.Parse(HttpContext.Session.GetString("UsuarioId"));

            if (rol != "Docente" && rol != "Administrador")
            {
                return Forbid();
            }

            ViewBag.Cursos = rol == "Administrador"
                ? _context.Cursos.ToList()
                : _context.Cursos.Where(c => c.DocenteId == usuarioId).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Evaluacion evaluacion)
        {
            _context.Evaluaciones.Add(evaluacion);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var evaluacion = _context.Evaluaciones
                .Include(e => e.Curso)
                .Include(e => e.Preguntas)
                .ThenInclude(p => p.Respuestas)
                .FirstOrDefault(e => e.Id == id);

            if (evaluacion == null)
            {
                return NotFound();
            }

            return View(evaluacion);
        }

        public IActionResult Rendir(int id)
        {
            var evaluacion = _context.Evaluaciones
                .Include(e => e.Preguntas)
                    .ThenInclude(p => p.Respuestas)
                .FirstOrDefault(e => e.Id == id);

            if (evaluacion == null) return NotFound();

            var model = new RendirEvaluacionViewModel
            {
                EvaluacionId = evaluacion.Id,
                Titulo = evaluacion.Titulo,
                Preguntas = evaluacion.Preguntas.Select(q => new PreguntaRespuestaViewModel
                {
                    PreguntaId = q.Id,
                    Texto = q.Texto,
                    Respuestas = q.Respuestas.Select(r => new RespuestaOption
                    {
                        RespuestaId = r.Id,
                        Texto = r.Texto
                    }).ToList()
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rendir(RendirEvaluacionViewModel model)
        {
            // Obtener el ID del estudiante desde la sesión
            var estudianteId = int.Parse(HttpContext.Session.GetString("UsuarioId"));

            // Calcular el puntaje
            int totalPreguntas = model.Preguntas.Count;
            int respuestasCorrectas = 0;

            foreach (var preguntaVM in model.Preguntas)
            {
                var respuestaCorrecta = _context.Respuestas
                    .Where(r => r.PreguntaId == preguntaVM.PreguntaId && r.EsCorrecta)
                    .FirstOrDefault();

                if (respuestaCorrecta != null && preguntaVM.RespuestaSeleccionada == respuestaCorrecta.Id)
                {
                    respuestasCorrectas++;
                }
            }

            double porcentaje = (double)respuestasCorrectas / totalPreguntas * 100;

            ViewBag.Resultado = $"Respondiste correctamente {respuestasCorrectas} de {totalPreguntas} preguntas. Tu puntaje es {porcentaje}%.";

            var nota = new Nota
            {
                EstudianteId = estudianteId,
                EvaluacionId = model.EvaluacionId,
                Calificacion = (decimal)porcentaje
            };

            _context.Notas.Add(nota);
            _context.SaveChanges();

            return View("Resultado");
        }

        public IActionResult Delete(int id)
        {
            var evaluacion = _context.Evaluaciones
                .Include(e => e.Curso)
                .FirstOrDefault(e => e.Id == id);

            if (evaluacion == null)
            {
                return NotFound();
            }

            return View(evaluacion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var evaluacion = _context.Evaluaciones.Find(id);

            if (evaluacion != null)
            {
                var preguntas = _context.Preguntas.Where(p => p.EvaluacionId == id).ToList();
                foreach (var pregunta in preguntas)
                {
                    var respuestas = _context.Respuestas.Where(r => r.PreguntaId == pregunta.Id).ToList();
                    _context.Respuestas.RemoveRange(respuestas);
                }
                _context.Preguntas.RemoveRange(preguntas);

                _context.Evaluaciones.Remove(evaluacion);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
