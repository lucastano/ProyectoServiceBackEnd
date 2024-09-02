using Microsoft.EntityFrameworkCore;
using ProyectoService.AccesoDatos.EntityFramework;
using ProyectoService.AccesoDatos;
using ProyectoService.LogicaNegocio.Modelo.ValueObjects;
using ProyectoService.LogicaNegocio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ProyectoService.LogicaNegocio.IRepositorios;

namespace ProyectoService.Test
{
    [TestFixture]
    public class TestAdministrador
    {

        private ProyectoServiceContext _context;
        private AdministradorEFRepositorio _administradorRepositorio;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ProyectoServiceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ProyectoServiceContext(options);
            SeedData();
            _administradorRepositorio = new AdministradorEFRepositorio(_context);
        }

        private void SeedData()
        {
            var salt = ObtenerSalt();
            var admin1 = new Administrador
            {
                Nombre = "Juan",
                Apellido = "Lopez",
                Email = EmailVO.Crear("juan@example.com"),
                PasswordHash = ObtenerHash("password", salt),
                PasswordSalt = salt,
                Rol = "Administrador"

            };

            var admin2 = new Administrador
            {
                Nombre = "Maria",
                Apellido = "Gomez",
                Email = EmailVO.Crear("maria@example.com"),
                PasswordHash = ObtenerHash("password", salt),
                PasswordSalt = salt,
                Rol = "Administrador"
            };

            _context.Administradores.AddRange(admin1, admin2);
            _context.SaveChanges();
        }



        [Test]
        public async Task Add_ShouldAddAdministrador()
        {
            // Arrange
            var admin = new Administrador
            {
                Nombre = "Carlos",
                Apellido = "Perez",
                Email = EmailVO.Crear("carlos@example.com"),
                PasswordHash = ObtenerHash("password", ObtenerSalt()),
                PasswordSalt = ObtenerSalt(),
                Rol = "Administrador"
            };

            // Act
            await _administradorRepositorio.Add(admin);

            // Assert
            var administradorAgregado = _context.Administradores.FirstOrDefault(c => c.Email == admin.Email);
            Assert.IsNotNull(administradorAgregado);
        }


        [Test]
        public async Task Add_ShouldThrowException_WhenInvalidEmail()
        {
            var ex = Assert.Throws<Exception>(() =>
            {
                Administrador admin = new Administrador
                {
                    Nombre = "Roberto",
                    Apellido = "Gonzalez",
                    Email = EmailVO.Crear("robertoexample.com"), // email invalido
                    PasswordHash = ObtenerHash("password", ObtenerSalt()),
                    PasswordSalt = ObtenerSalt(),
                    Rol = "Administrador"
                };
            });

            Assert.AreEqual("Email no valido", ex.Message);
        }
        //COMIENZA CAMBIO PASSWORD
        [Test]
        public async Task ChangePassword_ShouldUpdatePassword()
        {
            string emailAdministrador = "juan@example.com";
            string nuevaPassword = "Pr1234567";
            byte[] salt = ObtenerSalt();
            byte[] hash = ObtenerHash(nuevaPassword, salt);

            bool resultado = await _administradorRepositorio.CambiarPassword(emailAdministrador, hash, salt);

            Assert.True(resultado);
        }

        [Test]
        public async Task ChangePassword_ShouldReturnFalseWhenPasswordEmpty()
        {
            string emailAdministrador = "juan@example.com";
            //se le pasa salt y hash null, ya que la funcion los requiere
            byte[] salt = null;
            byte[] hash = null;
            bool resultado = await _administradorRepositorio.CambiarPassword(emailAdministrador, hash, salt);
            //debe retornar false
            Assert.False(resultado);
        }

        [Test]
        public async Task ChangePassword_ShouldReturnFalseWhenEmailEmpty()
        {
            //se le pasa todos los datos bien menos el email del tecnico
            string emailAdministrador = "";
            string nuevaPassword = "Pr1234567";

            //se le pasa salt y hash null, ya que la funcion los requiere
            byte[] salt = ObtenerSalt();
            byte[] hash = ObtenerHash(nuevaPassword, salt);
            bool resultado = await _administradorRepositorio.CambiarPassword(emailAdministrador, hash, salt);
            //debe retornar false
            Assert.False(resultado);
        }
        [Test]
        public async Task ChangePassword_ShouldReturnFalseWhenAdministradorNull()
        {
            //se le pasa todos los datos bien menos el email del tecnico, ese tecnico no existe
            string emailAdministrador = "ddd@gmail.com"; //tecnico es null porque no encuentra el tecnico con ese correo
            string nuevaPassword = "Pr1234567";
            //se le pasa salt y hash null, ya que la funcion los requiere
            byte[] salt = ObtenerSalt();
            byte[] hash = ObtenerHash(nuevaPassword, salt);
            bool resultado = await _administradorRepositorio.CambiarPassword(emailAdministrador, hash, salt);
            //debe retornar false
            Assert.False(resultado);
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
