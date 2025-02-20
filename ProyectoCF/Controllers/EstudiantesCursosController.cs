using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoCF.Models;

namespace ProyectoCF.Controllers
{
    public class EstudiantesCursosController : Controller
    {
        private readonly Connection _context;

        public EstudiantesCursosController(Connection context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var asignaciones = _context.EstudiantesCursos
                .Include(ec => ec.Estudiante)
                .Include(ec => ec.Curso)
                .ToList();

            return View(asignaciones);
        }

        public IActionResult Create()
        {
            ViewBag.Estudiantes = _context.Usuarios.Where(u => u.Rol == "Estudiante").ToList();
            ViewBag.Cursos = _context.Cursos.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EstudianteCurso estudianteCurso)
        {
                _context.EstudiantesCursos.Add(estudianteCurso);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var asignacion = _context.EstudiantesCursos
                .Include(ec => ec.Estudiante)
                .Include(ec => ec.Curso)
                .FirstOrDefault(ec => ec.Id == id);

            if (asignacion == null)
            {
                return NotFound();
            }

            return View(asignacion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var asignacion = _context.EstudiantesCursos.Find(id);
            if (asignacion != null)
            {
                _context.EstudiantesCursos.Remove(asignacion);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
