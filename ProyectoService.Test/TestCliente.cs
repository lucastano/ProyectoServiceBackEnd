using NUnit.Framework;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using ProyectoService.AccesoDatos;
using ProyectoService.AccesoDatos.EntityFramework;
using ProyectoService.LogicaNegocio.Modelo;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Security.Cryptography;
using System.Security.Cryptography;
using ProyectoService.LogicaNegocio.Modelo.ValueObjects;
using ProyectoService.LogicaNegocio.Excepciones;

namespace ProyectoService.Test
{
    [TestFixture]
    public class Tests
    {
        private ProyectoServiceContext _context;
        private ClienteEFRepositorio _clienteRepositorio;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ProyectoServiceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ProyectoServiceContext(options);
            SeedData();
            _clienteRepositorio = new ClienteEFRepositorio(_context);
        }

        private void SeedData()
        {
            var salt = ObtenerSalt();
            var cliente1 = new Cliente
            {
                Nombre = "Juan",
                Apellido = "Lopez",
                Email = EmailVO.Crear("juan@example.com"),
                PasswordHash = ObtenerHash("password", salt),
                PasswordSalt = salt,
                Rol = "Cliente",
                Direccion = "Calle Principal",
                Telefono = "123456789",
                Ci = "12345678"
            };

            var cliente2 = new Cliente
            {
                Nombre = "Maria",
                Apellido = "Gomez",
                Email = EmailVO.Crear("maria@example.com"),               
                PasswordHash = ObtenerHash("password", salt),
                PasswordSalt = salt,
                Rol = "Cliente",
                Direccion = "Calle Secundaria",
                Telefono = "987654321",
                Ci = "87654321"
            };

            _context.Clientes.AddRange(cliente1, cliente2);
            _context.SaveChanges();
        }

        [Test]
        public void Add_ShouldAddCliente()
        {
            // Arrange
            var cliente = new Cliente
            {
                Nombre = "Carlos",
                Apellido = "Perez",
                Email = EmailVO.Crear("carlos@example.com"),
                PasswordHash = ObtenerHash("password", ObtenerSalt()),
                PasswordSalt = ObtenerSalt(),
                Rol = "Cliente",
                Direccion = "Avenida Central",
                Telefono = "555555555",
                Ci = "98765432"
            };

            // Act
            _clienteRepositorio.Add(cliente);

            // Assert
            var clienteAgregado = _context.Clientes.FirstOrDefault(c => c.Email == cliente.Email);
            Assert.IsNotNull(clienteAgregado);
        }

        [Test]
        public async Task Add_ShouldThrowException_WhenInvalidCi()
        {// Arrange
            var cliente = new Cliente
            {
                Nombre = "Roberto",
                Apellido = "Gonzalez",
                Email = EmailVO.Crear("roberto@example.com"),
                PasswordHash = ObtenerHash("password", ObtenerSalt()),
                PasswordSalt = ObtenerSalt(),
                Rol = "Cliente",
                Direccion = "Avenida Norte",
                Telefono = "444444444",
                Ci = "12345" // CI inválida
            };

            // Act & Assert
            var ex =  Assert.ThrowsAsync<ClienteException>(async () => await _clienteRepositorio.Add(cliente));
            Assert.AreEqual("Número de documento inválido", ex.Message);
        }

        [Test]
        public async Task Add_ShouldThrowException_WhenInvalidEmail()
        {
            var ex = Assert.Throws<Exception>(() =>
            {
                Cliente cliente = new Cliente
                {
                    Nombre = "Roberto",
                    Apellido = "Gonzalez",
                    Email = EmailVO.Crear("robertoexample.com"), // email invalido
                    PasswordHash = ObtenerHash("password", ObtenerSalt()),
                    PasswordSalt = ObtenerSalt(),
                    Rol = "Cliente",
                    Direccion = "Avenida Norte",
                    Telefono = "444444444",
                    Ci = "12345678"
                };
            });

            Assert.AreEqual("Email no valido", ex.Message);
        }


        [Test]
        public async Task GetClienteByCi_ShouldReturnCorrectCliente()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ProyectoServiceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Creamos un cliente para agregarlo al contexto en memoria
          
            var expectedCliente = new Cliente
            {
                Nombre = "Roberto",
                Apellido = "Gonzalez",
                Email =EmailVO.Crear("roberto@example.com"),
                PasswordHash = ObtenerHash("password", ObtenerSalt()),
                PasswordSalt = ObtenerSalt(),
                Rol = "Cliente",
                Direccion = "Avenida Norte",
                Telefono = "444444444",
                Ci = "12345656" 
            };
            var ci = "12345656"; // CI válida de un cliente existente en la base de datos

            using (var context = new ProyectoServiceContext(options))
            {
                context.Clientes.Add(expectedCliente);
                context.SaveChanges();
            }

            // Act
            using (var context = new ProyectoServiceContext(options))
            {
                var repository = new ClienteEFRepositorio(context);
                var actualCliente = await repository.GetClienteByCi(ci);

                // Assert
                Assert.IsNotNull(actualCliente); // Verificar que el cliente devuelto no sea nulo
                Assert.AreEqual(expectedCliente.Nombre, actualCliente.Nombre); // Verificar otras propiedades del cliente devuelto según sea necesario
                                                                               // Verificar otras propiedades del cliente devuelto según sea necesario
            }
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
