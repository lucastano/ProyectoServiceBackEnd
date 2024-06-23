using Microsoft.EntityFrameworkCore;
using ProyectoService.AccesoDatos;
using ProyectoService.LogicaNegocio.Modelo.ValueObjects;
using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ProyectoService.AccesoDatos.EntityFramework;

namespace ProyectoService.Test
{
    [TestFixture]
    public class TestLogin
    {
        private ProyectoServiceContext _context;
        private AuthService _authService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ProyectoServiceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ProyectoServiceContext(options);
            SeedData();
            _authService = new AuthService(_context);
        }

        private void SeedData()
        {
            var clienteSalt = ObtenerSalt();
            var tecnicoSalt = ObtenerSalt();
            var adminSalt = ObtenerSalt();

            var cliente = new Cliente
            {
                Nombre = "Cliente",
                Apellido = "Perez",
                Email = EmailVO.Crear("cliente@example.com"),
                Ci = "46059980",
                PasswordHash = ObtenerHash("46059980", clienteSalt),
                PasswordSalt = clienteSalt,
                Rol = "Cliente",
                Direccion = "manzana 39 solar 17",
                Telefono = "095461141"
            };

            var tecnico = new Tecnico
            {
                Nombre = "Tecnico",
                Apellido = "Lopez",
                Email = EmailVO.Crear("tecnico@example.com"),
                PasswordHash = ObtenerHash("password", tecnicoSalt),
                PasswordSalt = tecnicoSalt,
                Rol = "Tecnico"
            };

            var administrador = new Administrador
            {
                Nombre = "Admin",
                Apellido = "Gomez",
                Email = EmailVO.Crear("admin@example.com"),
                PasswordHash = ObtenerHash("password", adminSalt),
                PasswordSalt = adminSalt,
                Rol = "Administrador"
            };

            _context.Clientes.Add(cliente);
            _context.Tecnicos.Add(tecnico);
            _context.Administradores.Add(administrador);
            _context.SaveChanges();
        }

        [Test]
        public async Task Login_ShouldReturnCliente_WhenEmailAndRoleAreCorrect()
        {
            // Act
            var usuario = await _authService.Login("cliente@example.com", "Cliente");

            // Assert
            Assert.IsNotNull(usuario);
            Assert.IsInstanceOf<Cliente>(usuario);
            Assert.AreEqual("Cliente", usuario.Nombre);
        }

        [Test]
        public async Task Login_ShouldReturnTecnico_WhenEmailAndRoleAreCorrect()
        {
            // Act
            var usuario = await _authService.Login("tecnico@example.com", "Tecnico");

            // Assert
            Assert.IsNotNull(usuario);
            Assert.IsInstanceOf<Tecnico>(usuario);
            Assert.AreEqual("Tecnico", usuario.Nombre);
        }

        [Test]
        public async Task Login_ShouldReturnAdministrador_WhenEmailAndRoleAreCorrect()
        {
            // Act
            var usuario = await _authService.Login("admin@example.com", "Administrador");

            // Assert
            Assert.IsNotNull(usuario);
            Assert.IsInstanceOf<Administrador>(usuario);
            Assert.AreEqual("Admin", usuario.Nombre);
        }

        [Test]
        public void Login_ShouldThrowException_WhenEmailIsNull()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _authService.Login(null, "Cliente"));
            Assert.AreEqual("Debe ingresar email", ex.Message);
        }

        [Test]
        public void Login_ShouldThrowException_WhenRoleIsNull()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _authService.Login("cliente@example.com", null));
            Assert.AreEqual("debe ingresar un rol de usuario valido", ex.Message);
        }

        [Test]
        public void Login_ShouldThrowException_WhenRoleIsInvalid()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _authService.Login("cliente@example.com", "InvalidRole"));
            Assert.AreEqual("debe ingresar un rol de usuario valido", ex.Message);
        }

        [Test]
        public void Login_ShouldThrowException_WhenEmailDoesNotExist()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _authService.Login("noexiste@example.com", "Cliente"));
            Assert.AreEqual("Email incorrectos", ex.Message);
        }

        private byte[] ObtenerSalt()
        {
            var salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private byte[] ObtenerHash(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                return pbkdf2.GetBytes(32);
            }
        }


    }
}
