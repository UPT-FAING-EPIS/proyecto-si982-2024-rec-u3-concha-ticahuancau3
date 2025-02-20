using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using ProyectoCF.Controllers;
using ProyectoCF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Necesario para ISession
using Moq;
using System.Text;

namespace ProyectoCF.Controllers.Tests
{
    [TestClass]
    public class UsuariosControllerTests
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
                context.Usuarios.AddRange(
                    new Usuario
                    {
                        Id = 1,
                        Nombre = "John",
                        Apellido = "Doe",
                        Email = "john.doe@example.com",
                        Contrasena = "password123",
                        Rol = "Estudiante"
                    },
                    new Usuario
                    {
                        Id = 2,
                        Nombre = "Jane",
                        Apellido = "Smith",
                        Email = "jane.smith@example.com",
                        Contrasena = "password456",
                        Rol = "Docente"
                    }
                );
                context.SaveChanges();
            }
        }

        // Prueba: Acceso denegado
        [TestMethod]
        public void CreateTest_AccesoDenegado_RetornaForbid()
        {
            // Arrange
            using (var context = new Connection(_options))
            {
                var controller = new UsuariosController(context);
                var usuario = new Usuario
                {
                    Nombre = "Alice",
                    Apellido = "Johnson",
                    Email = "alice.johnson@example.com",
                    Contrasena = "password789",
                    Rol = "Estudiante"
                };

                // Simular que no hay rol en la sesión
                var mockSession = new Mock<ISession>();
                mockSession.Setup(s => s.TryGetValue("Rol", out It.Ref<byte[]>.IsAny)) // Simular TryGetValue con byte[]
                    .Returns(false); // No hay valor para "Rol"
                var mockHttpContext = new Mock<HttpContext>();
                mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);
                controller.ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                };

                // Act
                var result = controller.Create(usuario) as ForbidResult;

                // Assert
                Assert.IsNotNull(result);
            }
        }

        // Prueba: Docente intenta crear un usuario que no es estudiante
        [TestMethod]
        public void CreateTest_DocenteIntentaCrearNoEstudiante_RetornaVistaConError()
        {
            // Arrange
            using (var context = new Connection(_options))
            {
                var controller = new UsuariosController(context);
                var usuario = new Usuario
                {
                    Nombre = "Alice",
                    Apellido = "Johnson",
                    Email = "alice.johnson@example.com",
                    Contrasena = "password789",
                    Rol = "Administrador" // Intentar crear un Administrador como Docente
                };

                // Simular que el rol en la sesión es Docente
                var rolBytes = Encoding.UTF8.GetBytes("Docente"); // Convertir el rol a byte[]
                var mockSession = new Mock<ISession>();
                mockSession.Setup(s => s.TryGetValue("Rol", out rolBytes)) // Simular TryGetValue con byte[]
                    .Returns(true); // Hay un valor para "Rol"
                var mockHttpContext = new Mock<HttpContext>();
                mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);
                controller.ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                };

                // Act
                var result = controller.Create(usuario) as ViewResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsFalse(controller.ModelState.IsValid);
                Assert.AreEqual(1, controller.ModelState.ErrorCount);
                Assert.AreEqual("Los docentes solo pueden crear estudiantes.", controller.ModelState.Values.First().Errors[0].ErrorMessage);
            }
        }

        // Prueba: Creación exitosa
        [TestMethod]
        public void CreateTest_CreacionExitosa_RetornaRedirectToAction()
        {
            // Arrange
            using (var context = new Connection(_options))
            {
                var controller = new UsuariosController(context);
                var usuario = new Usuario
                {
                    Nombre = "Alice",
                    Apellido = "Johnson",
                    Email = "alice.johnson@example.com",
                    Contrasena = "password789",
                    Rol = "Estudiante"
                };

                // Simular que el rol en la sesión es Administrador
                var rolBytes = Encoding.UTF8.GetBytes("Administrador"); // Convertir el rol a byte[]
                var mockSession = new Mock<ISession>();
                mockSession.Setup(s => s.TryGetValue("Rol", out rolBytes)) // Simular TryGetValue con byte[]
                    .Returns(true); // Hay un valor para "Rol"
                var mockHttpContext = new Mock<HttpContext>();
                mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);
                controller.ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                };

                // Act
                var result = controller.Create(usuario) as RedirectToActionResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.ActionName);

                // Verificar que el usuario fue agregado a la base de datos
                using (var contextVerify = new Connection(_options))
                {
                    var usuarios = contextVerify.Usuarios.ToList();
                    Assert.AreEqual(3, usuarios.Count); // 2 usuarios iniciales + 1 nuevo
                    Assert.IsTrue(usuarios.Any(u => u.Email == "alice.johnson@example.com"));
                }
            }
        }
    }
}