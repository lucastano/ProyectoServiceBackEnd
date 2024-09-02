using ProyectoService.AccesoDatos.EntityFramework;
using ProyectoService.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoService.LogicaNegocio.IRepositorios;
using ProyectoService.LogicaNegocio.Modelo.ValueObjects;
using ProyectoService.LogicaNegocio.Modelo;
using System.Security.Cryptography;

namespace ProyectoService.Test
{
    [TestFixture]
    public class TestTecnico
    {
        private ProyectoServiceContext _context;
        private TecnicoEFRepositorio _tecnicoRepositorio;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ProyectoServiceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ProyectoServiceContext(options);
            SeedData();
            _tecnicoRepositorio = new TecnicoEFRepositorio(_context);
        }

        private void SeedData()
        {
            var salt = ObtenerSalt();
            var tecnico1 = new Tecnico
            {
                Nombre = "Juan",
                Apellido = "Lopez",
                Email = EmailVO.Crear("juan@example.com"),
                PasswordHash = ObtenerHash("password", salt),
                PasswordSalt = salt,
                Rol = "Tecnico"
                
            };

            var tecnico2 = new Tecnico
            {
                Nombre = "Maria",
                Apellido = "Gomez",
                Email = EmailVO.Crear("maria@example.com"),
                PasswordHash = ObtenerHash("password", salt),
                PasswordSalt = salt,
                Rol = "Tecnico"
            };

            _context.Tecnicos.AddRange(tecnico1, tecnico2);
            _context.SaveChanges();
        }



        [Test]
        public void Add_ShouldAddTecnico()
        {
            // Arrange
            var tecnico = new Tecnico
            {
                Nombre = "Carlos",
                Apellido = "Perez",
                Email = EmailVO.Crear("carlos@example.com"),
                PasswordHash = ObtenerHash("password", ObtenerSalt()),
                PasswordSalt = ObtenerSalt(),
                Rol = "Tecnico"
            };

            // Act
            _tecnicoRepositorio.Add(tecnico);

            // Assert
            var tecnicoAgregado = _context.Tecnicos.FirstOrDefault(c => c.Email == tecnico.Email);
            Assert.IsNotNull(tecnicoAgregado);
        }


        [Test]
        public async Task Add_ShouldThrowException_WhenInvalidEmail()
        {
            var ex = Assert.Throws<Exception>(() =>
            {
                Tecnico tecnico = new Tecnico
                {
                    Nombre = "Roberto",
                    Apellido = "Gonzalez",
                    Email = EmailVO.Crear("robertoexample.com"), // email invalido
                    PasswordHash = ObtenerHash("password", ObtenerSalt()),
                    PasswordSalt = ObtenerSalt(),
                    Rol = "Tecnico"
                };
            });

            Assert.AreEqual("Email no valido", ex.Message);
        }

        [Test]
        public async Task ChangePassword_ShouldUpdatePassword()
        {
            string emailTecnico = "juan@example.com";
            string nuevaPassword = "Pr1234567";
            byte[]salt=ObtenerSalt();
            byte[]hash=ObtenerHash(nuevaPassword, salt);

            bool resultado = await _tecnicoRepositorio.CambiarPassword(emailTecnico, hash, salt);
            
            Assert.True(resultado);
        }

        [Test]
        public async Task ChangePassword_ShouldReturnFalseWhenPasswordEmpty()
        {
            string emailTecnico = "juan@example.com";
            //se le pasa salt y hash null, ya que la funcion los requiere
            byte[] salt = null;
            byte[] hash = null;
            bool resultado = await _tecnicoRepositorio.CambiarPassword(emailTecnico, hash, salt);
            //debe retornar false
            Assert.False(resultado);
        }

        [Test]
        public async Task ChangePassword_ShouldReturnFalseWhenEmailEmpty()
        {
            //se le pasa todos los datos bien menos el email del tecnico
            string emailTecnico = "";
            string nuevaPassword = "Pr1234567";

            //se le pasa salt y hash null, ya que la funcion los requiere
            byte[] salt = ObtenerSalt();
            byte[] hash = ObtenerHash(nuevaPassword, salt);
            bool resultado = await _tecnicoRepositorio.CambiarPassword(emailTecnico, hash, salt);
            //debe retornar false
            Assert.False(resultado);
        }
        [Test]
        public async Task ChangePassword_ShouldReturnFalseWhenTecnicoNull()
        {
            //se le pasa todos los datos bien menos el email del tecnico, ese tecnico no existe
            string emailTecnico = "ddd@gmail.com"; //tecnico es null porque no encuentra el tecnico con ese correo
            string nuevaPassword = "Pr1234567";
            //se le pasa salt y hash null, ya que la funcion los requiere
            byte[] salt = ObtenerSalt();
            byte[] hash = ObtenerHash(nuevaPassword, salt);
            bool resultado = await _tecnicoRepositorio.CambiarPassword(emailTecnico, hash, salt);
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
