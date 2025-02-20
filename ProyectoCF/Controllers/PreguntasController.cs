using Microsoft.AspNetCore.Mvc;
using ProyectoCF.Models;

namespace ProyectoCF.Controllers
{
    public class PreguntasController : Controller
    {
        private readonly Connection _context;

        public PreguntasController(Connection context)
        {
            _context = context;
        }

        public IActionResult Create(int evaluacionId)
        {
            var model = new PreguntaCreateViewModel
            {
                EvaluacionId = evaluacionId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PreguntaCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pregunta = new Pregunta
                {
                    EvaluacionId = model.EvaluacionId,
                    Texto = model.Texto
                };
                _context.Preguntas.Add(pregunta);
                _context.SaveChanges();

                var respuestas = new List<Respuesta>
                {
                    new Respuesta
                    {
                        PreguntaId = pregunta.Id,
                        Texto = model.Respuesta1,
                        EsCorrecta = (model.CorrectaIndex == 1)
                    },
                    new Respuesta
                    {
                        PreguntaId = pregunta.Id,
                        Texto = model.Respuesta2,
                        EsCorrecta = (model.CorrectaIndex == 2)
                    },
                    new Respuesta
                    {
                        PreguntaId = pregunta.Id,
                        Texto = model.Respuesta3,
                        EsCorrecta = (model.CorrectaIndex == 3)
                    },
                };

                _context.Respuestas.AddRange(respuestas);
                _context.SaveChanges();

                return RedirectToAction("Details", "Evaluaciones", new { id = model.EvaluacionId });
            }

            return View(model);
        }
    }
}
