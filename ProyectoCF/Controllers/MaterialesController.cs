using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoCF.Models;

namespace ProyectoCF.Controllers
{
    public class MaterialesController : Controller
    {
        private readonly Connection _context;

        public MaterialesController(Connection context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var rol = HttpContext.Session.GetString("Rol");
            var usuarioId = int.Parse(HttpContext.Session.GetString("UsuarioId"));

            if (rol == "Docente")
            {
                var materiales = _context.Materiales
                    .Include(m => m.Curso)
                    .Where(m => m.Curso.DocenteId == usuarioId)
                    .ToList();

                return View(materiales);
            }

            if (rol == "Administrador")
            {
                var materiales = _context.Materiales
                    .Include(m => m.Curso)
                    .ToList();

                return View(materiales);
            }

            return Forbid();
        }


        public IActionResult Create()
        {
            var rol = HttpContext.Session.GetString("Rol");
            var usuarioId = int.Parse(HttpContext.Session.GetString("UsuarioId"));

            if (rol == "Docente")
            {
                ViewBag.Cursos = _context.Cursos.Where(c => c.DocenteId == usuarioId).ToList();
                return View();
            }

            if (rol == "Administrador")
            {
                ViewBag.Cursos = _context.Cursos.ToList();
                return View();
            }

            return Forbid();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Material material)
        {
            _context.Materiales.Add(material);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            var usuarioId = int.Parse(HttpContext.Session.GetString("UsuarioId"));

            var material = _context.Materiales.Include(m => m.Curso).FirstOrDefault(m => m.Id == id);

            if (rol == "Docente" && material?.Curso.DocenteId != usuarioId)
            {
                return Forbid();
            }

            if (material == null)
            {
                return NotFound();
            }

            ViewBag.Cursos = _context.Cursos.Where(c => c.DocenteId == usuarioId).ToList();
            return View(material);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Material material)
        {
            _context.Materiales.Update(material);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var material = _context.Materiales.Find(id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var material = _context.Materiales.Find(id);
            if (material != null)
            {
                _context.Materiales.Remove(material);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
