using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoCF.Models;


namespace ProyectoCF.Controllers
{
    public class CursosController : Controller
    {
        private readonly Connection _context;

        public CursosController(Connection context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var rol = HttpContext.Session.GetString("Rol");
            var usuarioId = int.Parse(HttpContext.Session.GetString("UsuarioId"));

            ViewBag.Rol = rol;

            if (rol == "Docente")
            {
                var cursos = _context.Cursos
                    .Where(curso => curso.DocenteId == usuarioId)
                    .Select(curso => new CursoViewModel
                    {
                        Id = curso.Id,
                        Nombre = curso.Nombre,
                        Descripcion = curso.Descripcion,
                        DocenteNombre = _context.Usuarios
                            .Where(u => u.Id == curso.DocenteId)
                            .Select(u => u.Nombre + " " + u.Apellido)
                            .FirstOrDefault() ?? "No asignado"
                    })
                    .ToList();

                return View(cursos);
            }

            if (rol == "Administrador")
            {
                var cursos = _context.Cursos
                    .Select(curso => new CursoViewModel
                    {
                        Id = curso.Id,
                        Nombre = curso.Nombre,
                        Descripcion = curso.Descripcion,
                        DocenteNombre = _context.Usuarios
                            .Where(u => u.Id == curso.DocenteId)
                            .Select(u => u.Nombre + " " + u.Apellido)
                            .FirstOrDefault() ?? "No asignado"
                    })
                    .ToList();

                return View(cursos);
            }

            return Forbid();
        }


        public IActionResult Details(int id)
        {
            var curso = _context.Cursos
                .Where(c => c.Id == id)
                .Select(c => new Curso
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    DocenteId = c.DocenteId,
                    Materiales = _context.Materiales
                        .Where(m => m.CursoId == c.Id)
                        .ToList()
                })
                .FirstOrDefault();

            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }


        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador" && HttpContext.Session.GetString("Rol") != "Docente")
            {
                return Forbid();
            }

            ViewBag.Docentes = _context.Usuarios.Where(u => u.Rol == "Docente").ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Curso curso)
        {
                _context.Cursos.Add(curso);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            var usuarioId = int.Parse(HttpContext.Session.GetString("UsuarioId"));
            ViewBag.Rol = rol;

            if (rol != "Administrador" && rol != "Docente")
            {
                return Forbid();
            }
            var curso = _context.Cursos.Find(id);
            if (curso == null)
            {
                return NotFound();
            }

            if (rol == "Docente" && curso.DocenteId != usuarioId)
            {
                return Forbid();
            }

            if (rol == "Administrador")
            {
                ViewBag.Docentes = _context.Usuarios
                    .Where(u => u.Rol == "Docente")
                    .ToList();
            }

            return View(curso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Curso curso)
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador" && HttpContext.Session.GetString("Rol") != "Docente")
            {
                return Forbid();
            }
            _context.Cursos.Update(curso);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador" && HttpContext.Session.GetString("Rol") != "Docente")
            {
                return Forbid();
            }

            var curso = _context.Cursos.Find(id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador" && HttpContext.Session.GetString("Rol") != "Docente")
            {
                return Forbid();
            }

            var curso = _context.Cursos.Find(id);
            if (curso != null)
            {
                _context.Cursos.Remove(curso);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
