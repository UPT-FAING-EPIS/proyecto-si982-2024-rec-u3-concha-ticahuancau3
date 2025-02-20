using Microsoft.AspNetCore.Mvc;
using ProyectoCF.Models;
using System.Linq;

namespace ProyectoCF.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Connection _context;

        public UsuariosController(Connection context)
        {
            _context = context;
        }

        private bool EsAccesoPermitido()
        {
            var rolActual = HttpContext.Session.GetString("Rol");
            return rolActual == "Administrador" || rolActual == "Docente";
        }

        public IActionResult Index()
        {
            if (!EsAccesoPermitido())
            {
                return Forbid();
            }

            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!EsAccesoPermitido())
            {
                return Forbid();
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            if (!EsAccesoPermitido())
            {
                return Forbid();
            }

            string rolActual = HttpContext.Session.GetString("Rol");

            if (rolActual == "Docente" && usuario.Rol != "Estudiante")
            {
                ModelState.AddModelError("", "Los docentes solo pueden crear estudiantes.");
                return View(usuario);
            }

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!EsAccesoPermitido())
            {
                return Forbid();
            }

            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }


        [HttpPost]
        public IActionResult Edit(Usuario usuario)
        {
            if (!EsAccesoPermitido())
            {
                return Forbid();
            }

            string rolActual = HttpContext.Session.GetString("Rol");

            if (rolActual == "Docente" && usuario.Rol != "Estudiante")
            {
                ModelState.AddModelError("", "Los docentes solo pueden editar estudiantes.");
                return View(usuario);
            }

            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!EsAccesoPermitido())
            {
                return Forbid();
            }

            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            string rolActual = HttpContext.Session.GetString("Rol");
            if (rolActual == "Docente" && usuario.Rol != "Estudiante")
            {
                return Forbid();
            }

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
