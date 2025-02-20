using Microsoft.AspNetCore.Mvc;
using ProyectoCF.Models;
using System.Linq;

namespace ProyectoCF.Controllers
{
    public class AuthController : Controller
    {
        private readonly Connection _context;

        public AuthController(Connection context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string contrasena)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Contrasena == contrasena);
            if (usuario != null)
            {
                HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
                HttpContext.Session.SetString("Rol", usuario.Rol);
                HttpContext.Session.SetString("Nombre", usuario.Nombre);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View();
        }


        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}
