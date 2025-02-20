using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoCF.Models;

namespace ProyectoCF.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Connection _context;

    public HomeController(ILogger<HomeController> logger, Connection context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Rol")) ||
            string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId")))
        {
            return RedirectToAction("Login", "Auth");
        }

        var usuarioId = int.Parse(HttpContext.Session.GetString("UsuarioId"));
        var rol = HttpContext.Session.GetString("Rol");
        var nombre = HttpContext.Session.GetString("Nombre");

        ViewBag.Rol = rol;
        ViewBag.Nombre = nombre;

        if (rol == "Docente")
        {
            var cursos = _context.Cursos
                .Where(c => c.DocenteId == usuarioId)
                .ToList();
            ViewBag.Cursos = cursos.Select(c => new CursoViewModel
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                DocenteNombre = nombre
            }).ToList();
        }
        else if (rol == "Estudiante")
        {
            var cursos = _context.EstudiantesCursos
                .Include(ec => ec.Curso)
                .Where(ec => ec.EstudianteId == usuarioId)
                .Select(ec => ec.Curso)
                .ToList();
            ViewBag.Cursos = cursos.Select(c => new CursoViewModel
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                DocenteNombre = _context.Usuarios.FirstOrDefault(u => u.Id == c.DocenteId)?.Nombre ?? "No asignado"
            }).ToList();
        }

        return View();
    }



    public IActionResult Privacy()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Rol")))
        {
            return RedirectToAction("Login", "Auth");
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Rol")))
        {
            return RedirectToAction("Login", "Auth");
        }

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
