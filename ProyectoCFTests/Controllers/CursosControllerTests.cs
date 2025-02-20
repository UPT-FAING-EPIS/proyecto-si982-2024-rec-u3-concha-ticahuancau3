using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using ProyectoCF.Controllers;
using ProyectoCF.Models;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoCFTests.Controllers
{
    [TestClass]
    public class CursosControllerTests
    {
        private DbContextOptions<Connection> _options;

        // Configuración inicial: Crear una base de datos en memoria
        [TestInitialize]
        public void Setup()
        {
            // Configurar opciones para usar una base de datos en memoria
            _options = new DbContextOptionsBuilder<Connection>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Nombre único para evitar conflictos
                .Options;

            // Agregar datos de prueba a la base de datos en memoria
            using (var context = new Connection(_options))
            {
                context.Cursos.AddRange(
                    new Curso
                    {
                        Id = 1,
                        Nombre = "Educación para el Trabajo - 1° \"A\" - Secundaria",
                        Descripcion = "El área Educación para el Trabajo - EPT...",
                        DocenteId = 2,
                        Materiales = new List<Material>()
                    },
                    new Curso
                    {
                        Id = 3,
                        Nombre = "Educación para el Trabajo - 1° \"B\" - Secundaria",
                        Descripcion = "El área Educación para el Trabajo - EPT...",
                        DocenteId = 5,
                        Materiales = new List<Material>()
                    }
                );
                context.SaveChanges();
            }
        }

        // Prueba: Detalles de un curso existente
        [TestMethod]
        public void DetailsTest_CursoExiste_RetornaVistaConDatos()
        {
            // Arrange
            using (var context = new Connection(_options))
            {
                var controller = new CursosController(context);

                // Act
                var result = controller.Details(1) as ViewResult;
                var model = result?.Model as Curso;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(model);
                Assert.AreEqual(1, model.Id);
                Assert.AreEqual("Educación para el Trabajo - 1° \"A\" - Secundaria", model.Nombre);
                Assert.AreEqual(2, model.DocenteId); // Verificar el DocenteId
                Assert.AreEqual(0, model.Materiales.Count); // Verificar que no hay materiales asociados
            }
        }

        // Prueba: Detalles de un curso inexistente
        [TestMethod]
        public void DetailsTest_CursoNoExiste_RetornaNotFound()
        {
            // Arrange
            using (var context = new Connection(_options))
            {
                var controller = new CursosController(context);

                // Act
                var result = controller.Details(999);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }
        }
    }
}